using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TradeMark.Models;

namespace TradeMark.Utility
{
    public class StripePayment
    {
        /// <summary>
        /// normal one time payment with stripe gateway
        /// </summary>
        /// <param name="token">Token created with the card detail pass here</param>
        /// <returns>return chagredId/transactionId</returns>
        public StripeCharge MakePayement(UserTransactionModel objUserTransactionModel)
        {
            try
            {
                var myCharge = new StripeChargeCreateOptions();
                myCharge.Amount = Convert.ToInt32(objUserTransactionModel.Amount);
                myCharge.Currency = ConfigurationManager.AppSettings["CurrencyName"].ToString();
                myCharge.Description = "To buy search credit";
                myCharge.CustomerId = objUserTransactionModel.StripeCustomerId;

                var chargeService = new StripeChargeService(ConfigurationManager.AppSettings["App.Stripe.SourceKey"].ToString());
                return chargeService.Create(myCharge);
            }
            catch (Exception exe)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// Method to create stripe customer
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="tokenno"></param>
        /// <returns></returns>
        public StripeCustomer CreateCutomer(string emailid, string tokenno)
        {
            var mycust = new StripeCustomerCreateOptions();

            mycust.Email = emailid;
            mycust.Description = "Payment for the: " + emailid;

            mycust.SourceToken = tokenno;
            var customerservice = new StripeCustomerService(ConfigurationManager.AppSettings["App.Stripe.SourceKey"].ToString());

            return customerservice.Create(mycust);
        }
        /// <summary>
        /// Refund the amount
        /// </summary>
        /// <param name="chargeId">pass the id for which we have to refund amount</param>
        /// <returns>return refund id</returns>
        //public string MakeRefund(string chargeId, int amount)
        //{
        //    var refund = new StripeRefundCreateOptions();
        //    refund.Amount = amount;

        //    refund.Metadata = new Dictionary<string, string>() { { "key", "value" } };
        //    var refundService = new StripeRefundService(ConfigurationManager.AppSettings["App.Stripe.SourceKey"].ToString());
        //    var refundCharge = refundService.Create(chargeId, refund);
        //    var refundId = refundCharge.Id;
        //    return refundId;
        //}
        ///// <summary>
        /////  provide customer a required subscription
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public StripeSubscription CustomerSubscription(SubscriptionInput input)
        //{
        //    var sourceKey = Configuration.AppSettings.AppStripeSourceKeyIndividual; ;
        //    // var palnId = "CodeSidePlan";
        //    if (input.SubscriberType == 1)
        //    {
        //        sourceKey = Configuration.AppSettings.AppStripeSourceKeyBusiness;
        //    }

        //    var stripeSubscription = new StripeSubscriptionService(sourceKey);

        //    var stripeSubscriptionObj = new StripeSubscriptionCreateOptions();
        //    return stripeSubscription.Create(input.CustomerId, input.PlanId);

        //}
        ///// <summary>
        ///// create a customer on the stripe
        ///// </summary>
        ///// <param name="token">token generated with respect to the card detail</param>
        ///// <param name="palnId">Plan Id for which user subscribe</param>
        ///// <param name="Email">user Email address</param>
        ///// <param name="discription">discription of payment</param>
        ///// <returns>StripeCustomer detail</returns>
        //public StripeCustomer CreateCustomer(SubscriptionInput input)
        //{
        //    try
        //    {
        //        var sourceKey = Configuration.AppSettings.AppStripeSourceKeyIndividual;
        //        var palnId = "CodeSidePlan";
        //        if (input.SubscriberType == 1)
        //        {
        //            sourceKey = Configuration.AppSettings.AppStripeSourceKeyBusiness;
        //        }
        //        var stripeCustomer = new StripeCustomerCreateOptions();
        //        stripeCustomer.Email = input.Email;
        //        stripeCustomer.Description = input.Description;
        //        stripeCustomer.SourceToken = input.Token;
        //        stripeCustomer.PlanId = input.PlanId;
        //        var customerService = new StripeCustomerService(sourceKey);
        //        var customer = customerService.Create(stripeCustomer);
        //        return customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        ///// <summary>
        /////  create the subscription plan here
        ///// </summary>
        ///// <returns></returns>
        //public string CreateSubscriptionPlan(SubscriptionInput input)
        //{

        //    try
        //    {
        //        var sourceKey = Configuration.AppSettings.AppStripeSourceKeyIndividual;
        //        var currency = Configuration.AppSettings.CurrencyNameCH;
        //        // var palnId = "CodeSidePlan";
        //        if (input.SubscriberType == 1)
        //        {
        //            sourceKey = Configuration.AppSettings.AppStripeSourceKeyBusiness;
        //            currency = Configuration.AppSettings.CurrencyNameAus;
        //        }
        //        var subscriptionService = new StripePlanService(sourceKey);
        //        var subscription = new StripePlanCreateOptions();
        //        subscription.Amount = input.Amount;
        //        subscription.Currency = currency;
        //        subscription.Interval = input.Interval;
        //        subscription.IntervalCount = input.IntervalCount;
        //        subscription.Id = input.PlanId;
        //        subscription.Name = input.PlanName;

        //        var createdSubscription = subscriptionService.Create(subscription);
        //        return createdSubscription.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;

        //    }
        //}
        ///// <summary>
        ///// Update subscription plan
        ///// </summary>
        ///// <returns></returns>
        //public string UpdateSubscriptionPlan()
        //{

        //    var subscriptionService = new StripePlanService(ConfigurationManager.AppSettings["App.Stripe.SourceKey"].ToString());
        //    var subscription = new StripePlanUpdateOptions();
        //    subscription.Name = "CodeSidePlan1";
        //    var updatedPlan = subscriptionService.Update("CodeSidePlan", subscription);
        //    return "";
        //}
        ///// <summary>
        ///// Delete subscription plan
        ///// </summary>
        ///// <returns></returns>
        ///// 
        //public bool DeleteSubscriptionPlan(SubscriptionInput input)
        //{
        //    var sourceKey = Configuration.AppSettings.AppStripeSourceKeyIndividual;

        //    if (input.SubscriberType == 1)
        //    {
        //        sourceKey = Configuration.AppSettings.AppStripeSourceKeyBusiness;
        //    }
        //    var subscriptionService = new StripePlanService(sourceKey);
        //    if (!string.IsNullOrEmpty(input.PlanId))
        //    {
        //        try
        //        {
        //            // delete the subscription plan
        //            var deleteSubscription = subscriptionService.Delete(input.PlanId);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //            throw;
        //        }

        //    }
        //    return false;
        //}
        ///// <summary>
        ///// Cancel subscription of user
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public string CancelSubscription(SubscriptionInput input)
        //{
        //    var sourceKey = Configuration.AppSettings.AppStripeSourceKeyIndividual;

        //    if (input.SubscriberType == 1)
        //    {
        //        sourceKey = Configuration.AppSettings.AppStripeSourceKeyBusiness;
        //    }
        //    var stripeSubscription = new StripeSubscriptionService(sourceKey);
        //    var cancelSubscription = stripeSubscription.Cancel(input.CustomerId, stripeSubscription.List(input.CustomerId).ToList()[0].Id);
        //    return cancelSubscription.Status;
        //}
    }
}