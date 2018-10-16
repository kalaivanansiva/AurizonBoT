using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Drawing;
using System.Configuration;
using Newtonsoft.Json;
using SimpleEchoBot;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.ApplicationInsights.DataContracts;
using System.Web;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        private const string EntityDateTime = "builtin.datetimeV2.daterange";
        private const string ImgUrl = "http://sfdhr.org/sites/default/files/images/Employees/employee-leaves.jpg";
        private const string leaveTypesEntity = "leavetypes";
        private const string leaveBalanceEntity = "leavebalance";
        private const string personalEntity= "PersonalInfo";
         
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }
        

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context,"Sorry, I did not understand "+result.Query+". Type 'help' if you need assistance.");
        }
         [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context,"Thank you..!!");
        }
        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {   
           try
           {                   
            List<string> helpOption=new List<string>(new string[]{ "Apply leaves","Leave Balance","Access ESS","Payroll deduction","Novated Lease", "Change Personal details","Change bank details"});          
            PromptDialog.Choice(context, this.OnOptionSelected, helpOption, "I can help you with below options!!", "Not a valid option", 3);
           }
            catch(Exception ex)
           {
            WebApiApplication.Telemetry.TrackException(ex);           
           }  
        }  
        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
           try
           {
             string optionSelected = await result;
           }
            catch (TooManyAttemptsException ex)
            {
                WebApiApplication.Telemetry.TrackException(ex);
                await context.PostAsync($"Ooops! Too many attemps :(. You can try again!");
        
               context.Wait(MessageReceived);
            }
        }       
        
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            try{
            // var telemetry = context.CreateTraceTelemetry(nameof(GreetingIntent), new Dictionary<string, string> { { @"SetDefault", bool.FalseString }, {@"UserMessage", result.Query.ToString()} });
            var query=result.Query;
             var telemetry = new TraceTelemetry("Trace Intents", SeverityLevel.Information);
        telemetry.Properties.Add("ConversationInput", query);        
            
            WebApiApplication.Telemetry.TrackTrace(telemetry);
            
            
            string message=$"How Can I help You?";
            string endConvoEntity = "endConvo";
            EntityRecommendation endConvoEntityRecommendation;            
            if(result.TryFindEntity(endConvoEntity, out endConvoEntityRecommendation))
            {
                 message=$"Thank you!!.. Have a nice day!!";
            }               
            await this.ShowLuisResult(context,message);
            }
            catch(Exception ex)
            {
                 WebApiApplication.Telemetry.TrackException(ex);
            }
        }      

        [LuisIntent("Leaves")]
        public async Task Leaves(IDialogContext context, LuisResult result)
        {
                        
            var query=result.Query;
            var replyMessage = context.MakeMessage();
           EntityRecommendation levBalEntityRecommendation;

            if (result.Entities.Count < 1)
            {               
                    if(query.Contains("types"))
                    {
                        replyMessage.Text ="Types of Leave \n\n 1. Sick leave\n2. Casual leave\n3. Earned leave\n4. Flexi leave";
                        await context.PostAsync(replyMessage);
                        return;
                    }  
                    
                    if(query.Contains("leave") || query.Contains("leaves"))  
                    { 
                    List<string> leaves=new List<string>(new string[]{ "Sick leave" ,"Casual leave", "Earned leave","Flexi leave"});  
                    PromptDialog.Choice(context, this.OnOptionSelected, leaves, "What type of leave do you want to apply?", "Not a valid option", 3);
                    return;
                    }
                    
                    replyMessage.Text="Sorry I did not understand. Please type 'Help' if you need any assistance";
            }
            else  if (result.TryFindEntity(leaveBalanceEntity, out levBalEntityRecommendation))
                {
                    replyMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;    
                    replyMessage.Attachments=await getLeaveBalance();   
                    await context.PostAsync(replyMessage);
                    return;                   
                } 

            else
            {
                EntityRecommendation levTypesEntityRecommendation;
                string link = "https://www.ultimatix.net/ ";
                if (result.TryFindEntity(leaveTypesEntity, out levTypesEntityRecommendation))
                {
                    IList<EntityRecommendation> entityList = result.Entities;
                    foreach (var entity in entityList)
                    {
                         if(entity.Type.Equals(leaveTypesEntity))
                        replyMessage.Text = "Click on this link " + link + "to apply " + entity.Entity + " leave.";

                    }
                }

            }
            await context.PostAsync(replyMessage);
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("AccessESS")]
        public async Task AccessESS(IDialogContext context, LuisResult result)
        {
            var query=result.Query;
            var replyMessage = context.MakeMessage();
            replyMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var signinCard = new SigninCard
            {
                Text = "ESS Sign-in",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in", value: "https://www.ultimatix.net/") }
            };

            replyMessage.Attachments.Add(signinCard.ToAttachment());
            
            WebApiApplication.Telemetry.TrackEvent(@"AceessESS", new Dictionary<string, string> { { @"UserInput", query }});
                                   
            await context.PostAsync(replyMessage);
        }            

        [LuisIntent("PersonalDetails")]
        public async Task PersonalDetails(IDialogContext context, LuisResult result)
        {
            var replyMessage = context.MakeMessage();
            var link = "www.changepersonalinfo.com";

            replyMessage.Text = "Please click on this link " + link + " to change your personal details";
            await context.PostAsync(replyMessage);
        }

        [LuisIntent("BankDetails")]
        public async Task BankDetails(IDialogContext context, LuisResult result)
        {            
            var link = "www.changebank.com";
            string replyMessage = "Please click on this link " + link + " to change you bank details in ESS";
            await this.ShowLuisResult(context,replyMessage);
        }
        
        [LuisIntent("NovatedLease")]
        public async Task NovatedLease(IDialogContext context, LuisResult result)
        {
           
            string replyMessage = "A Novated Lease is a three way agreement between you (the employee), your employer and StreetFleet ";           
            await this.ShowLuisResult(context,replyMessage);
        }
        
        [LuisIntent("PayrollDeduction")]
        public async Task PayrollDeduction(IDialogContext context, LuisResult result)
        {
            var replyMessage = context.MakeMessage();            
            if(result.Query.Contains("Change"))
            {
                replyMessage.Text="To make any change, file a new form W-4 with your employer";
            }
            else
            {
                replyMessage.Text = "Amount withheld by an employer from employee's earnings";
            }
            
            await context.PostAsync(replyMessage);
        }
        
        [LuisIntent("SalarySacrifice")]
        public async Task SalarySacrifice(IDialogContext context, LuisResult result)
        {
            var replyMessage = context.MakeMessage();
            replyMessage.Text = "You give up part of your salary and, in return, your employer gives you a non-cash benefit, such as childcare vouchers, or increased pension contributions ";
            await context.PostAsync(replyMessage);
        }

		[LuisIntent("Payslip")]
		public async Task Payslip(IDialogContext context, LuisResult result)
        {
			try{
			var replyMessage = context.MakeMessage();

            Attachment attachment = null;

            if (result.Entities.Count>0)
            {
                //DateTime startdate,enddate;
                EntityRecommendation payslipEntityRecommendation;

                if (result.TryFindEntity(EntityDateTime, out payslipEntityRecommendation))
                {
                    //  IList<EntityRecommendation> payslipEntityList = result.Entities;
                    //  foreach (var entity in payslipEntityList)
                    //  {
                    //      foreach (var vals in entity.Resolution?.Values)
                    //      {
                    //          if (((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("type").ToString() == "daterange")
                    //          {
                    //              startdate = (DateTime)((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("start");
                    //              enddate = (DateTime)((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("end");
                    //              replyMessage.Text = startdate.ToString() + enddate.ToString();
                    //          }
                    //      }
                    //  }
                    attachment = GetLocalFileAttachment();
                    replyMessage.Attachments = new List<Attachment> { attachment };
                }

            }
            else
            {
                 replyMessage.Text = "Please provide month and year (MON YYYY)";
            }
            await context.PostAsync(replyMessage);
            context.Wait(MessageReceived);
            }catch(Exception ex)
            {
                 WebApiApplication.Telemetry.TrackException(ex);
            }						
        }
               
        private static async Task<IList<Attachment>> getLeaveBalance()
        {
            try
            {
            List<Attachment> attachments = new List<Attachment>();
            LoansModel loanlist = new LoansModel();           
                    
            //leaveBalancelist=  await RestService.GetLeavesAsync<LeaveBalanceModel>("http://www.mocky.io/v2/5b2a1d9a3000000e009cd194");
           loanlist=  await RestService.GetLeavesAsync<LoansModel>("https://aem901-publish.adobesandbox.com/content/ACEBank/en/index/loans/home-loans.model.json");
           
          // CustomisedModel _customisedModel = new CustomisedModel();
           
           HeroCard heroCard = new HeroCard
           {
                //CF_LOAN
                Title = loanlist._items.root._items.cf_loan.title,
                 Text= loanlist._items.root._items.cf_loan.description,
           Images = new List<CardImage> { new CardImage("https://aem901-publish.adobesandbox.com/"+loanlist._items.root._items.cf_loan.elements.loanImage.value) },  
          // list of buttons   
          Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan.elements.knowMoreBtn.value , value: "https://google.com") , 
              new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan.elements.applyNowBtn.value , value: "https://google.com")
          }

           };
                //_customisedModel.Image=deserializedResult._items.root._items.cf_loan.elements.loanImage.value;
            HeroCard heroCard1 = new HeroCard
           {
                //CF_LOAN
                Title = loanlist._items.root._items.cf_loan_1529340239.title,
                 Text= loanlist._items.root._items.cf_loan_1529340239.description,
   Images = new List<CardImage> { new CardImage("https://aem901-publish.adobesandbox.com/"+loanlist._items.root._items.cf_loan_1529340239.elements.loanImage.value) },  
          
 Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan_1529340239.elements.knowMoreBtn.value , value: "https://google.com") , 
              new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan_1529340239.elements.applyNowBtn.value , value: "https://google.com")
          }


           };    
            HeroCard heroCard2 = new HeroCard
           {
                //CF_LOAN
                Title = loanlist._items.root._items.cf_loan_632079441.title,
                 Text= loanlist._items.root._items.cf_loan_632079441.description,
   Images = new List<CardImage> { new CardImage("https://aem901-publish.adobesandbox.com/"+loanlist._items.root._items.cf_loan_632079441.elements.loanImage.value) },  
          // list of buttons   
          Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan_632079441.elements.knowMoreBtn.value , value: "https://google.com") , 
              new CardAction(ActionTypes.OpenUrl,loanlist._items.root._items.cf_loan_632079441.elements.applyNowBtn.value , value: "https://google.com")
          }


           };    
attachments.Add(heroCard.ToAttachment());
attachments.Add(heroCard1.ToAttachment());
attachments.Add(heroCard2.ToAttachment());
           

                //CF_LOAN_1529340239
//                  _customisedModel.title = deserializedResult._items.root._items.cf_loan_1529340239.title;
//                  _customisedModel.Description = deserializedResult._items.root._items.cf_loan_1529340239.description;
//                  _customisedModel.Image = deserializedResult._items.root._items.cf_loan_1529340239.elements.loanImage.value;                
//                  heroCard.Add(_customisedModel);

//                  //CF_LOAN_632079441
//                  _customisedModel.title = deserializedResult._items.root._items.cf_loan_632079441.title;
//                  _customisedModel.Description = deserializedResult._items.root._items.cf_loan_632079441.description;
//                  _customisedModel.Image = deserializedResult._items.root._items.cf_loan_632079441.elements.loanImage.value;
//                  heroCard.Add(_customisedModel);


// foreach(var lev in leaveBalancelist.LeaveBalance)
           
           /*foreach(var lev in leaveBalancelist.items.cf_loan)
            {
                HeroCard heroCard = new HeroCard
                {
                    
                    Title = lev.elements.loanName,
                    //  Text = lev.LeavesRemaining.ToString(),   
                    //  Images = new List<CardImage> { new CardImage(lev.ImageUrl)}
                };
                attachments.Add(heroCard.ToAttachment());
            }*/
                return attachments;
            }
                        
            catch(Exception ex)
            {
                WebApiApplication.Telemetry.TrackException(ex);
                return null;}
        }
        
        private static Attachment GetLocalFileAttachment()
        {  
            
            var pdfPath = HttpContext.Current.Server.MapPath("~/Images/Payslip.pdf"); 
            Attachment attachment = new Attachment(); 
            attachment.ContentType = "application/pdf"; 
            attachment.ContentUrl = pdfPath; 
            attachment.Name = "Click to download Payslip"; 
            return attachment;                   
        }
   
        private async Task ShowLuisResult(IDialogContext context,string replyMessage) 
        {
            await context.PostAsync(replyMessage);
            context.Wait(MessageReceived);
        }        
     
    }
    
}