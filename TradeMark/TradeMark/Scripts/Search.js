$(function () {
$('#checkbox').click(function () {
    
    // to not show alert box.
    if ($('#checkbox').is(':checked')) {
        var now = new Date();
        var time = now.getTime();
        time += 3600 * 1000;
        now.setTime(time);
        document.cookie = "CheckboxCheck=yes; expires=" + now.toUTCString();
    } else {

        var now = new Date();

        document.cookie = "CheckboxCheck=yes; expires=" + now.toUTCString();
    }
});
});

function noenter() {
    return !(window.event && window.event.keyCode == 13);
}

function ResetSearchPanel(value) {
    
    if (value == 3) {
        document.getElementById("divComponentFullForm").style.display = '';
        //  $('#spnEnterMark').html("Enter an acronym, nickname, or abbreviation.");
        $("#ContentPlaceHolder1_hdnRdoOption").val("3");
        $("#spnComponentFullForm").html("Identify the long form of the mark.");
        
    }
    if (value == 2) {
        $("#ContentPlaceHolder1_hdnRdoOption").val("2");
        document.getElementById("divComponentFullForm").style.display = '';
        $('#spnEnterMark').html("");
        $("#spnComponentFullForm").html("You can enter multiple components separated by comma(,)");
    }
    if (value == 1) {
        $("#ContentPlaceHolder1_hdnRdoOption").val("1");
        document.getElementById("divComponentFullForm").style.display = 'none';
        $('#spnEnterMark').html("");
        $("#spnComponentFullForm").html("");
        $("#ContentPlaceHolder1_txtComponentFullForm").val("");
    }
}
// To show and hide the text box
function TextBoxShow(value) {

    if (value == 3) {
        document.getElementById("divComponentFullForm").style.display = '';
        //  $('#spnEnterMark').html("Enter an acronym, nickname, or abbreviation.");
        $("#ContentPlaceHolder1_hdnRdoOption").val("3");
        $("#spnComponentFullForm").html("Identify the long form of the mark.");
        $("#ContentPlaceHolder1_txtComponentFullForm").val("");

        //
        $('#spnFullForm').html("");
        document.getElementById("divHybride").style.display = 'none';
    }
    if (value == 2) {
        $("#ContentPlaceHolder1_hdnRdoOption").val("2");
        document.getElementById("divComponentFullForm").style.display = '';
        $('#spnEnterMark').html("");
        $("#spnComponentFullForm").html("You can enter multiple components separated by comma(,)");
        $("#ContentPlaceHolder1_txtComponentFullForm").val("");
        //
        $('#spnFullForm').html("");
        document.getElementById("divHybride").style.display = 'none';
    }
    if (value == 4) {

        $("#ContentPlaceHolder1_hdnRdoOption").val("4");
        document.getElementById("divComponentFullForm").style.display = '';
        $('#spnEnterMark').html("");
        $("#spnComponentFullForm").html("");
        $("#ContentPlaceHolder1_txtComponentFullForm").val("");
        $("#spnComponentFullForm").html("You can enter multiple components separated by comma(,)");

        document.getElementById("divHybride").style.display = '';
        $("#spnFullForm").html("Identify the long form of the mark.");
    }
    if (value == 1) {
        $("#ContentPlaceHolder1_hdnRdoOption").val("1");
        document.getElementById("divComponentFullForm").style.display = 'none';
        $('#spnEnterMark').html("");
        $("#spnComponentFullForm").html("");
        $("#ContentPlaceHolder1_txtComponentFullForm").val("");
        $("#spnFullForm").html("");
        document.getElementById("divHybride").style.display = 'none';
        $("#ContentPlaceHolder1_txtFullForm").val("");

    }

}
// to make checkbox work as a radio button
function MutExChkList(chk) {

    if (chk.checked) {
        TextBoxShow(chk.value);
    }
    else {
        TextBoxShow(1);
    }

    var chkList = chk.parentNode.parentNode.parentNode;

    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {
        if (chks[i] != chk && chk.checked) {


            chks[i].checked = false;
        }
    }
}

$(document).ready(function () {
    
    // DropDown chooser
    $(".chosen-select").chosen({ no_results_text: "Oops, nothing found!" });
    // assign width to dropdown
    $("#ContentPlaceHolder1_ddlGoodsServices_chosen").css({ "width": "400" });

    $("input:radio").addClass("radio-custom");

    $("input:radio").next('label').addClass("radio-custom-label");
    if ($("#ContentPlaceHolder1_CheckBoxList1_3").is(':checked')) {
        
        document.getElementById("divComponentFullForm").style.display = '';
        document.getElementById("divHybride").style.display = '';
        
        //  $('#spnEnterMark').html("Enter an acronym, nickname, or abbreviation.");
        $("#ContentPlaceHolder1_hdnRdoOption").val("4");
        $("#spnComponentFullForm").html("You can enter multiple components separated by comma(,)");
        $("#spnFullForm").html("Identify the long form of the mark.");
    }
    
    if ($("#ContentPlaceHolder1_CheckBoxList1_2").is(':checked'))
    {
        
        document.getElementById("divComponentFullForm").style.display = '';
        //  $('#spnEnterMark').html("Enter an acronym, nickname, or abbreviation.");
        $("#ContentPlaceHolder1_hdnRdoOption").val("3");
        $("#spnComponentFullForm").html("Identify the long form of the mark.");
    }
    if ($("#ContentPlaceHolder1_CheckBoxList1_1").is(':checked'))
    {
       
     $("#ContentPlaceHolder1_hdnRdoOption").val("2");
    document.getElementById("divComponentFullForm").style.display = '';
    $('#spnEnterMark').html("");
    $("#spnComponentFullForm").html("You can enter multiple components separated by comma(,)");
    }
    // check the checkbox according to its value.
    if ($("#ContentPlaceHolder1_hdnSearchoption").val() == "2") {
       
        $("#ContentPlaceHolder1_CheckBoxList1_1").prop('checked', true);
    }
    if ($("#ContentPlaceHolder1_hdnSearchoption").val() == "3") {
 
        $("#ContentPlaceHolder1_CheckBoxList1_2").prop('checked', true);
    }
    if ($("#ContentPlaceHolder1_hdnSearchGuid").val() != "") {
        //if user comes from the History 
        ResetSearchPanel($("#ContentPlaceHolder1_hdnSearchoption").val());
    } else {
        //Search page
        TextBoxShow($("#ContentPlaceHolder1_hdnSearchoption").val());
    } 
});



//function to validate Trademark text to perform search
function ValidateTMtext() {
    
    document.getElementById("spnTMtxtError").style.display = 'none';
    document.getElementById("spnGoodsDatatxtError").style.display = 'none';
    $('#ContentPlaceHolder1_lblTitle').text("");
    $('#ContentPlaceHolder1_txtTitle').val("");

    if ($("#autoGoodsData").val() == "") {
        $("#spnGoodsDatatxtError").text("Required");
        document.getElementById("spnGoodsDatatxtError").style.display = '';
        $("#ContentPlaceHolder1_hdnGoodsID").val("");
        $("#ContentPlaceHolder1_hdnGoodsName").val("");


    }
    if ($("#ContentPlaceHolder1_hdnGoodsID").val() == "" && $("#autoGoodsData").val() != "") {
        document.getElementById("spnGoodsDatatxtError").style.display = '';
        $("#spnGoodsDatatxtError").text("Input data is invalid");
        $("#ContentPlaceHolder1_hdnGoodsID").val("");
        $("#ContentPlaceHolder1_hdnGoodsName").val("");
    }

    //if ($("#ContentPlaceHolder1_txtMark").val() == "") {
    if ($("#ContentPlaceHolder1_txtMark").val() == "") {
        document.getElementById("spnTMtxtError").style.display = '';

    }

    if ($("#ContentPlaceHolder1_hdnGoodsID").val() == "" || $("#autoGoodsData").val() == "" || $("#ContentPlaceHolder1_txtMark").val() == "") {
        return false;
    }

    else {
        sessionStorage['searchedGoodsName'] = $("#ContentPlaceHolder1_hdnGoodsName").val();


        document.getElementById("spnTMtxtError").style.display = 'none';
        document.getElementById("spnGoodsDatatxtError").style.display = 'none';
        //document.getElementById("divloader").style.display = '';
        //document.getElementById("divSearchResult").style.display = 'none';
        if (document.cookie.indexOf("visited") >= 0) {
            if (document.cookie.indexOf("CheckboxCheck") >= 0) {
                
                $("#ContentPlaceHolder1_btnSearch").click();
            }
            else {
                
                // They've been here before. 
                if ($("#ContentPlaceHolder1_CheckBoxList1_1").is(':checked') || $("#ContentPlaceHolder1_CheckBoxList1_2").is(':checked')) {
                    $("#btnOpenalertModel").click();
                    //$("#ContentPlaceHolder1_btnSearch").click();
                }
                else {

                    $("#btnOpenalertModel").click();
                }
            }
        }
        else {
            // create the cookies here
            var now = new Date();
            var time = now.getTime();
            time += 3600 * 1000;
            now.setTime(time);
            document.cookie = "visited=yes; expires=" + now.toUTCString();
            $("#ContentPlaceHolder1_btnSearch").click();

        }

        return false;
    }
}



//function to validate trademark search while saving Save Search 
function ValidateTMtextSaveSearch() {

    $('#ContentPlaceHolder1_lblTitle').text("");
    $('#ContentPlaceHolder1_txtTitle').val("");

    //if ($("#ContentPlaceHolder1_txtMark").val() == "") {
    if ($("#ContentPlaceHolder1_txtMark").val() == "") {

        document.getElementById("spnTMtxtError").style.display = '';
    }

    if ($("#autoGoodsData").val() == "" || $("#ContentPlaceHolder1_hdnGoodsID").val() == "") {
        $("#spnGoodsDatatxtError").text("Required");
        document.getElementById("spnGoodsDatatxtError").style.display = '';
        $("#ContentPlaceHolder1_hdnGoodsID").val("");
        $("#ContentPlaceHolder1_hdnGoodsName").val("");
    }
    else {
        if (sessionStorage['searchedGoodsName'] != $("#ContentPlaceHolder1_hdnGoodsName").val()) {

            alert("Sorry you can not save this search, name of the searched data and the current select goods name are different");
            return false;
        }

        document.getElementById("spnTMtxtError").style.display = 'none';
        $("#btnOpenModel").click();
    }
    return false;
}



//Function to save search trademark selected parameters
function SaveSearchData() { //This function call on text change.

    
    var pointofContact;
    var markIndex = $("#ContentPlaceHolder1_txtMark").val();
    var componentFullForm = $("#ContentPlaceHolder1_txtComponentFullForm").val();
    var fullForm = $("#ContentPlaceHolder1_txtFullForm").val();

    // if ($('input[id*=rdoMark]').is(":checked"))
    if ($("#ContentPlaceHolder1_hdnRdoOption").val() == "1") {
        pointofContact = 1;
    }
    // if ($('input[id*=rdocomponents]').is(":checked")) 
    if ($("#ContentPlaceHolder1_hdnRdoOption").val() == "2") {
        pointofContact = 2;
    }
    // if ($('input[id*=rdofullform]').is(":checked"))
    if ($("#ContentPlaceHolder1_hdnRdoOption").val() == "3") {
        pointofContact = 3;
    }
    if ($("#ContentPlaceHolder1_hdnRdoOption").val() == "4") {
        pointofContact = 4;
    }

    var Title = $("#ContentPlaceHolder1_txtTitle").val();

    //var goodsServices = $("#ContentPlaceHolder1_ddlGoodsServices").val();
    var goodsServices = $("#ContentPlaceHolder1_hdnGoodsID").val();


    if (markIndex != null && markIndex != "" && Title != null && Title != "") {
        $("#btnSaveSearch").hide();
        $.ajax({
            //saving search
            url: 'AjaxHandler.aspx?status=SaveSearchresult&markIndex=' + markIndex + '&componentFullForm=' + componentFullForm + '&pointofContact=' + pointofContact + '&Title=' + Title + '&goodsServices=' + goodsServices + '&fullForm=' + fullForm, // this for calling the web method function in cs code. 
            //contentType: "application/json; charset=utf-8",
            dataType: 'text',
            success:
                function (result) {

                    if (result === "true") {
                        document.getElementById("spnMsg").style.display = '';
                        //Success message 
                        setTimeout($('#myModal').hide, 10000);
                        setTimeout(window.location.href = 'search.aspx', 15000);
                    }
                    else {
                    }
                }
        })
    }
    else {

        $('#ContentPlaceHolder1_lblTitle').html(" Required!")
        //$("#ContentPlaceHolder1_lblTitle").css({ "color": "red" });
        $("#ContentPlaceHolder1_lblTitle").addClass("lbl-red");

    }
}
function CheckPopUpresult() {
    $("#ContentPlaceHolder1_hdnSearchRslt").val("true");

}
function showComponentFullText() {
    document.getElementById("divComponentFullForm").style.display = '';

}
//function to show credits after perform a search
function showCredits(userCredits) {

    $("#lblUserCredits").text(userCredits);
    if(userCredits==1)
    {
        
        $("#liAutoreload").show();
    } else {
        $("#liAutoreload").hide();
    }
}

function showloader(showhide) {
    
    
    if (showhide == 'show') {
        
        document.getElementById("divloader").style.display = '';
        document.getElementById("ContentPlaceHolder1_divSearchResult").style.display = 'none';
        
        
        $('#myAlertModal').hide();
        $('.modal-backdrop').hide();
    }
    if (showhide == 'hide') {
        document.getElementById("divloader").style.display = 'none';
        document.getElementById("ContentPlaceHolder1_divSearchResult").style.display = '';
    }

}

function funSetDocContent() {

    $("#ContentPlaceHolder1_hdnSearchResult").val(document.getElementById("ContentPlaceHolder1_divSearchResult").innerHTML);
}

function FunSearchresult() {

    $.ajax({
        //saving search
        url: 'Search.aspx/SearchtradeMark', // this for calling the web method function in cs code. 
        //contentType: "application/json; charset=utf-8",
        dataType: 'text',
        success:
            function (result) {

                if (result === "true") {
                    document.getElementById("spnMsg").style.display = '';
                    //Success message 
                    setTimeout($('#myModal').hide, 10000);
                    setTimeout(window.location.href = 'search.aspx', 15000);
                }
                else {
                }
            }
    })
    // SearchtradeMark
}

//function to hide send email to Attorney message
function showSuccessEmailMsg() {
    
    setTimeout(function () { document.getElementById("ContentPlaceHolder1_lblSuccessmsg").style.display = 'none' }, 10000);
}
//Function to generate the HTML to export into Doc
function generateFinalHtmlToExport() {
    $("#ContentPlaceHolder1_hdnSearchResult").val(document.getElementById("ContentPlaceHolder1_divSearchResult").innerHTML);
    var appBaseurl = $("#ContentPlaceHolder1_hdnApplicationUrl").val();
    
    var imgSrc = appBaseurl + "Images/warningImage.JPG";
    var searchResult = document.getElementById('ContentPlaceHolder1_hdnSearchResult').value;
    
    if (searchResult.indexOf("redLightMsg") >= 0) {
        searchResult = searchResult.replace('class=\"redLightMsg', 'Style=\"color:red;');
        imgSrc = appBaseurl + "Images/warningImage.JPG";
    }
    else if (searchResult.indexOf("yellowLightMsg") >= 0) {
        searchResult = searchResult.replace('class=\"yellowLightMsg', ' Style=\"color: #fff1a7;');
        imgSrc = appBaseurl + "Images/cautionImage.JPG";
    }
    else if (searchResult.indexOf("redLightMsg") >= 0) {
        searchResult = searchResult.replace('class=\"GreenLightMsg', ' Style=\"color: green;');
        imgSrc = appBaseurl + "Images/congratulationImage.JPG";
    }
    var stringHeader = "<center><img src='" + imgSrc + "'/></center><div><table><tr><td>Mark Searched: </td><td> <b>" + document.getElementById('ContentPlaceHolder1_txtMark').value + "</b></td></tr><tr><td>Goods/Services Searched: </td><td><b>" + document.getElementById('ContentPlaceHolder1_hdnGoodsName').value + "</b></td></tr><tr><td>Date Searched: </td><td><b>" + $('#ContentPlaceHolder1_hdnCurrentDate').val() + "</b></td></tr></table></div>";

    var finalString = stringHeader + searchResult;

    $("#ContentPlaceHolder1_hdnSearchResultCopy").val(searchResult)
    $("#ContentPlaceHolder1_hdnSearchResult").val(finalString);

}
//Export to Doc
function downloadDoc() {

    var uri = 'data:application/ms-word,' + encodeURIComponent('<html><body>' + $("#ContentPlaceHolder1_hdnSearchResult").val() + '</body></html>');
    var downloadLink = document.createElement("a");
    downloadLink.href = uri;
    downloadLink.download = "trademarkSerachResult.doc";

    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

var AllGoodsServices;

$(document).ready(function () {

    $.ajax({
        type: "POST",
        url: 'Search.aspx?value=Goods',
        async: false,
        success: function (data) {

            var jsonString = data;
            jsonString = jsonString.replace(/\s+/g, " ");
            obj = JSON && JSON.parse(jsonString) || $.parseJSON(jsonString);
            AllGoodsServices = obj;
            var arr = [];

            for (var index in AllGoodsServices) {
                var content = AllGoodsServices[index];
                arr.push(content.USClassDescription);
            }

            $("#autoGoodsData").autocomplete({
                source: arr,
                autoFocus: true,
                minLength: 1,
                max: 10,
                scroll: true,
                select: function (event, ui) {

                    for (var index in AllGoodsServices) {
                        var content = AllGoodsServices[index];

                        if (content.USClassDescription == ui.item.value) {
                            
                            document.getElementById("ContentPlaceHolder1_hdnGoodsID").value = content.UsClassno;
                            document.getElementById("ContentPlaceHolder1_hdnGoodsName").value = content.USClassDescription;

                            break;
                        }


                    }
                }
            });
        }
    });

    document.getElementById("autoGoodsData").value = document.getElementById("ContentPlaceHolder1_hdnGoodsName").value;
    if (document.getElementById("ContentPlaceHolder1_hdnGoodsID").value != '' && document.getElementById("ContentPlaceHolder1_hdnGoodsID").value != null) {
        for (var index in AllGoodsServices) {
            var content = AllGoodsServices[index];

            if (content.UsClassno == document.getElementById("ContentPlaceHolder1_hdnGoodsID").value) {

                document.getElementById("ContentPlaceHolder1_hdnGoodsID").value = content.UsClassno;
                document.getElementById("ContentPlaceHolder1_hdnGoodsName").value = content.USClassDescription;
                document.getElementById("autoGoodsData").value = content.USClassDescription;
                break;
            }


        }
    }

});






