var Range2to9 = 94.99;
var Range10to19 = 93.99;
var Range20to29 = 92.99;
var Range30to39 = 91.99;
var Range40to49 = 90.99;
var Range50to59 = 89.99;
var Range60to69 = 88.99
var Range70to79 = 87.99;
var Range80to89 = 86.99;
var Range90to99 = 85.99;
var Range100 = 84.99;
var Range1000 = 19.99
var IsClick = false;

//Effect Per Name Cost
function PriceCalculationTotalNoSearchUsed() {
    $("#ContentPlaceHolder1_txtSearchPerCost").val(" ");
    var status = PriceCalculationTotalNumber();
    var noOFName = $("#ContentPlaceHolder1_txtTotalCost").val().replace("$", " ").trim();
    var totalSearch = $("#ContentPlaceHolder1_txtNoOFName").val();
    var valueTotalNoSearchUsed = $("#ContentPlaceHolder1_txtTotalNoSearchUsed").val();
    $("#spnErrortxtTotalNoSearchUsed").text(" ");
    if (status == true) {

        var regex = /^[0-9]*$/;
        var re = new RegExp(regex);
        var status = re.test(valueTotalNoSearchUsed)
        if (status == true) {
            if (parseInt(valueTotalNoSearchUsed) > 0 && parseInt(valueTotalNoSearchUsed) <= parseInt(totalSearch)) {
                if (noOFName != "" || noOFName != "Error") {
                    var amount = (parseFloat(noOFName) / parseInt(valueTotalNoSearchUsed)).toFixed(2);
                    $("#ContentPlaceHolder1_txtSearchPerCost").val("$"+amount);
                }
                else {

                }

            }
            else {
                $("#spnErrortxtTotalNoSearchUsed").text("Number must be greater then 0 and less then total number of names.");
            }
        }
        else {
            $("#spnErrortxtTotalNoSearchUsed").text("Please enter numbers only.")
        }
    }
    else {
        $("#spnErrortxtTotalNoSearchUsed").text("Please enter the valid total number of names.")
    }
}
function PriceCalculationTotalNumber() {
    //$("#ContentPlaceHolder1_txtTotalNoSearchUsed").val(" ");
    //$("#ContentPlaceHolder1_txtSearchPerCost").val(" ");
    $("#spnErrortxtNoOFName").text(" ");
    var value = $("#ContentPlaceHolder1_txtNoOFName").val();
    var IsValid = true;
    $("#ContentPlaceHolder1_txtTotalCost").val(" ")
    if (value != "") {
        var regex = /^[0-9]*$/;
        var re = new RegExp(regex);
        var status = re.test(value)
        if (status == true) {
            if (parseInt(value) > 1) {
                var calculatedValue = CustomPriceTool(value)
                
                if (calculatedValue == "Error") {
                    $("#ContentPlaceHolder1_txtTotalCost").val(calculatedValue);
                } else {
                    $("#ContentPlaceHolder1_txtTotalCost").val("$" + calculatedValue.toFixed(2));
                }
                if (parseInt(value) > 1 && parseInt(value) <= 100) {
                    //$("#ContentPlaceHolder1_btnBuy").removeClass("hide");
                    $("#btnBuy").removeClass("hide");

                }
                else {
                    $("#btnBuy").addClass("hide");
                }
            }
            else {
                $("#spnErrortxtNoOFName").text("Number must be greater then 1.");
                IsValid = false;
            }
        }
        else {
            $("#spnErrortxtNoOFName").text("Please enter numbers only.")
            IsValid = false;
        }
    }
    else {
        IsValid = false;
    }
    return IsValid;
}

function CustomPriceTool(value) {
    //if (parseInt(value) == 1) {
    //    return parseInt(value) * 99.99
    if (parseInt(value) >= 2 && parseInt(value) <= 9) {
        return parseInt(value) * parseFloat(Range2to9)

    }
    if (parseInt(value) >= 10 && parseInt(value) <= 19) {
        return parseInt(value) * parseFloat(Range10to19)

    }
    if (parseInt(value) >= 20 && parseInt(value) <= 29) {
        return parseInt(value) * parseFloat(Range20to29)
    }
    if (parseInt(value) >= 30 && parseInt(value) <= 39) {
        return parseInt(value) * parseFloat(Range30to39)
    }
    if (parseInt(value) >= 40 && parseInt(value) <= 49) {
        return parseInt(value) * parseFloat(Range40to49)
    }
    if (parseInt(value) >= 50 && parseInt(value) <= 59) {
        return parseInt(value) * parseFloat(Range50to59)
    }
    if (parseInt(value) >= 60 && parseInt(value) <= 69) {
        return parseInt(value) * parseFloat(Range60to69)
    }
    if (parseInt(value) >= 70 && parseInt(value) <= 79) {
        return parseInt(value) * parseFloat(Range70to79)
    }
    if (parseInt(value) >= 80 && parseInt(value) <= 89) {
        return parseInt(value) * parseFloat(Range80to89)
    }
    if (parseInt(value) >= 90 && parseInt(value) <= 99) {
        return parseInt(value) * parseFloat(Range90to99)
    }
    if (parseInt(value) == 100) {
        return parseInt(value) * parseFloat(Range100)
    }
    if (parseInt(value) >= 101 && parseInt(value) <= 999) {
        return "Error"
    }
    if (parseInt(value) >= 1000) {
        return parseInt(value) * parseFloat(Range1000)
    }
}



function MakePayment() {
    var promoCode = "";
    var credits = $("#ContentPlaceHolder1_txtNoOFName").val()

    var amount = $("#ContentPlaceHolder1_txtTotalCost").val()
    if (amount != "" || amount != "Error") {
        var url = "PaymentForm.aspx?amount=" + encodeURIComponent(amount) + "&credits=" + encodeURIComponent(credits) + "&promoCode=" + encodeURIComponent(promoCode);

        window.location.href = url;
    }

}
//function MakePaymentForMoreSearch(credits, noOFName, promoCode) {
//    var noOFName = $("#ContentPlaceHolder1_txtTotalCost").val().replace("$", " ").trim();
//    if (noOFName != "" || noOFName != "Error") {
//        var url = "PaymentForm.aspx?amount=" + encodeURIComponent(noOFName) + "&credits" + credits + "&promoCode" + promoCode;
//        window.location.href = url;
//    }

//}

function ApplyCoupan() {
    IsClick = true;
    var noOFName = "99.99"
    var promoCode = $("#ContentPlaceHolder1_txtApplyPromoCode").val();
    var credits = "1";
    $("#spnErrortxtPromocode").text(" ");
    if (promoCode != "") {
        $.ajax({
            "url": 'AjaxHandler.aspx?status=CheckPromoCode&PromoCode=' + promoCode,// this for calling the web method function in cs code.
            "dataType": "json",
            success: function (data) {
                console.log(data.PromoCodeId)
                if (data.PromoCodeId > 0) {
                    $("#ContentPlaceHolder1_hdnApplyCoupan").val("true");
                    $("#ContentPlaceHolder1_hdnAmount").val(data.Amount);
                    $("#spnErrortxtPromocode").removeClass('lbl-red');
                    $("#spnErrortxtPromocode").addClass('success').text("Your promo code is apply successfully.");
                }
                else {
                    $("#ContentPlaceHolder1_hdnApplyCoupan").val("false");
                    $("#ContentPlaceHolder1_hdnAmount").val(data.Amount);
                    $("#spnErrortxtPromocode").removeClass('success');
                    $("#spnErrortxtPromocode").addClass('lbl-red').text("Please enter valid promo code.");
                }
            },
            failure: function (response) {

            }
        })
    }
    //else {
    //    if (noOFName != "" || noOFName != "Error") {
    //        var url = "PaymentForm.aspx?amount=" + encodeURIComponent(noOFName) + "&credits=" + credits + "&promoCode=" + promoCode;
    //        window.location.href = url;
    //    }
    //}
}
function MakePaymentforsingleSearch() {
    var credits = "1";
    var promoCode = $("#ContentPlaceHolder1_txtApplyPromoCode").val();
    var status = $("#ContentPlaceHolder1_hdnApplyCoupan").val();
    var amount = $("#ContentPlaceHolder1_hdnAmount").val();
    if (promoCode != "") {
        if (status == "true") {
            var url = "PaymentForm.aspx?amount=" + encodeURIComponent(amount) + "&credits=" + encodeURIComponent(credits) + "&promoCode=" + encodeURIComponent(promoCode);
            window.location.href = url;
        }
    }
    else {
        $("#spnErrortxtPromocode").text(" ");
        amount = "99.99"
        var url = "PaymentForm.aspx?amount=" + encodeURIComponent(amount) + "&credits=" + encodeURIComponent(credits) + "&promoCode=" + encodeURIComponent(promoCode);
        window.location.href = url;
    }
}