function FormValidation()
{
    var isValid = true;
    var expiration = $("#ContentPlaceHolder1_txtExpiration").val();
    var CVC = $("#ContentPlaceHolder1_txtCVC").val();
    var CardNo = $("#ContentPlaceHolder1_txtCard").val();
    //var regexp = /^(1[0-2]|0[1-9]|\d)\/(20\d{2}|19\d{2}|0(?!0)\d|[1-9]\d)$/
    //var re = new RegExp(regexp);
    //alert(re.test(expiration));
    if (expiration != "") {
        //if (re.test(expiration) == false) {
        //    $("#spnErrorExpiration").text("Please enter expiration date in correct format.");
        //    isValid = false;
        //}
    }
    else {
        $("#spnErrorExpiration").text("Required.");
    }
    //    else {
           
    //        var status = Stripe.card.validateExpiry(expiration[0], "20" + expiration[1]);
    //        console.log(status);
    //        if (!Stripe.card.validateExpiry(expiration[0], "20" + expiration[1])) {
    //            $("#spnErrorExpiration").text("Please enter valid expiration date.")
    //            isValid = false;
    //        }
    //    }
    //}
    //else {
    //    $("#spnErrorExpiration").val("Required");
    //}
    //if (!Stripe.card.validateCVC(CVC))
    //{
    //    $("#spnErrorCVC").text("Please enter valid CVC number.")
    //    isValid=false;
    //}
    //if (Stripe.card.cardType(CardNo) == "Unknown")
    //{
    //    $("#spnErrorCard").text("Please enter valid card number.")
    //    isValid=false
    //}
    //if(isValid==true)
    //{

    //}
    IsValid();
}

//$(document).ready(function () {

//    Stripe.setPublishableKey("pk_test_fy6sfQ80sDTWtgORM2xlVbxY");
//})



//function IsValid() {

    
//   // if (($('#txtCardNumber').val() != "") && $('#txtCvc').val() != "" && $('#txtExpiration').val() != "") {

//    var expiration = $("#ContentPlaceHolder1_txtExpiration").val().split('/');;
//        debugger;
//        var status = Stripe.card.createToken({
//            number:$("#ContentPlaceHolder1_txtCard").val(),
//            cvc:  $("#ContentPlaceHolder1_txtCVC").val(),
//            exp_month: expiration[0],
//            exp_year: expiration[1]
//        }, stripeResponseHandler);
//        // form.submit();
//        return true;
//    //}
//    //else {
//    //    return false;
//    //}


//}

//function stripeResponseHandler(status, response) {

//    // grab the form:
//    if (response.error) { // problem!


//    } else { // token was created!

//        // get the token id:
//        var token = response.id;
//        alert(token);
//        console.log(token);

//        if (token != "") {

//        }
//        // Form.submit();

//    }
//};

$('document').ready(function () {
    Stripe.setPublishableKey('pk_test_P2Gswn7XDHmJr7GLu8h1tWrh');

    $('#btnCharge').on('click', function (e) {
        $("#spnErrorCard").text(" ");
        $("#spnErrorExpiry").text(" ");
        $("#spnErrorCVC").text(" ");
        var expiration = $('#txtExpiry').val();
        var expirationDetail = $('#txtExpiry').val().split("/");
        var CVC = $('#txtCvc').val();
        var CardNo = $('#txtCardNumber').val();
        var isValid = true;
        e.preventDefault();
        e.stopPropagation();
        if (CardNo == "")
        {
            $("#spnErrorCard").text("Required");
            isValid = false;
        }
        else {
            if (Stripe.card.cardType(CardNo) == "Unknown")
            {
                $("#spnErrorCard").text("Please enter valid card number.");
                isValid = false;
            }
        }
        if (CVC == "") {
            $("#spnErrorCVC").text("Required");
            isValid = false;
        }
        else {
            if (!Stripe.card.validateCVC(CVC)) {
                $("#spnErrorCVC").text("Please enter valid CVC number.");
                isValid = false;
            }
        }

        if (expiration == "") {
            $("#spnErrorExpiry").text("Required");
            isValid = false;
        }
        else {
            var regexp = /^(1[0-2]|0[1-9]|\d)\/(20\d{2}|19\d{2}|0(?!0)\d|[1-9]\d)$/
            var re = new RegExp(regexp);
          
            if (!re.test(expiration))
            {
                $("#spnErrorExpiry").text("Please enter valid expiry date.");
                isValid = false;
            }
            else if (!Stripe.card.validateExpiry(expirationDetail[0], "20" + expirationDetail[1])) {
                $("#spnErrorExpiry").text("Please enter valid expiry date.");
                isValid = false;
            }
              
        }

        if ($('#CheckBoxSubscriptionAgreement').is(':checked') == false) {
            $('#spnSubscriptionAgreement').text("Required!");
              isValid = false;
          }


        if (isValid == true) {
            Stripe.card.createToken({
                number: CardNo,
                cvc: CVC,
                exp_month: expirationDetail[0],
                exp_year: expirationDetail[1]
            }, stripeResponseHandler);
        }
    });

    function stripeResponseHandler(status, response) {
       // var $form = $('#frmCharge');

        if (response.error) {
            // Show the errors on the form
            alert(response.error.message);
        } else {
            // response contains id and card, which contains additional card details
            var token = response.id;
          
            // Insert the token into the form so it gets submitted to the server
            $('#ContentPlaceHolder1_hdnToken').val(token);
          
            var amount = $("#ContentPlaceHolder1_hdnAmount").val().replace("$"," ").trim();
            var credits = $("#ContentPlaceHolder1_hdnCredits").val();
            var promoCode = $("#ContentPlaceHolder1_hdnPromoCode").val();
           
            if((token!=""|| token!=null)&& (amount!=""||amount!=null))
            {
                //amount = parseFloat(amount) * 100;
                //if (promoCode == "")
                //{
                //    promoCode="NA"
                //}
                MakePayment(token, amount, credits, promoCode);
            }
            // and submit
           // $form.get(0).submit();
        }
    }
});

function MakePayment(token, amount, credits, promoCode) {
    $.ajax({
        "url": 'AjaxHandler.aspx?status=MakePayment&Token=' + token + '&Amount=' + amount + '&Credits=' + credits + '&PromoCode=' + promoCode,// this for calling the web method function in cs code.
        "dataType": "json",
        success: function (response) {
            console.log(response);
            var url = "PaymentStatus.aspx?PaymentStatus=" + encodeURIComponent(response.PaymentStatus) + "&Credits=" + encodeURIComponent(response.Credits) + "&TransactionId=" + encodeURIComponent(response.TransactionId);

            window.location.href = url;
           
        },
        failure: function (response) {
          
        }
    })

}

