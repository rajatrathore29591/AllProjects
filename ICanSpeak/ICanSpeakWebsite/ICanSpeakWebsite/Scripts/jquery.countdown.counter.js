(function ($) {
    "use strict";

    var gTicksLeft = 0;

    var digit1 = 0;
    var digit2 = 0;
    var digit3 = 0;
    var digit4 = 0;

    var gIntervalToken = null;

    var getTicksLeft = function () {
        return gTicksLeft;
    };

    var decTicksLeft = function () {
        gTicksLeft--;
    };

    var removeAllDigits = function ($element) {
        $element.removeClass("digit0 digit1 digit2 digit3 digit4 digit5 digit6 digit7 digit8 digit9");
    };

    var setItem = function (itemNumber, digit) {
        var token = "#counter_item" + itemNumber + " :first-child";
        var $element = $(token).next(); // second child

        removeAllDigits($element);
        $element.addClass("digit" + digit);
    };

    var calculateDigits = function () {
        var minutesLeft = Math.floor(getTicksLeft() / 60);
        var secondsLeft = getTicksLeft() - minutesLeft * 60;

        digit1 = Math.floor(minutesLeft / 10);
        digit2 = minutesLeft - digit1 * 10;

        digit3 = Math.floor(secondsLeft / 10);
        digit4 = secondsLeft - digit3 * 10;

        //$("#log").text("minutes left: " + minutesLeft + " | seconds left: " + secondsLeft + " | digits: " + digit1 + digit2 + ":" + digit3 + digit4);
    };

    var init = function () {
        calculateDigits();
        setItem(1, digit1);
        setItem(2, digit2);
        setItem(4, digit3);
        setItem(5, digit4);
    };

    var switchItem = function (itemNumber, digit, capacity) {
        var nextDigit = (digit === 0) ? capacity : (digit - 1);

        //$("#log2").text("digit" + digit + ", next digit: " + nextDigit);

        var token = "#counter_item" + itemNumber + " :first-child";
        var $element = $(token).next(); // second child

        removeAllDigits($element);
        $element.addClass("digit" + digit);
        $element.after('<div class="digit digit' + nextDigit + '" style="margin-top: 55px"></div>');

        var $newElement = $element.next();
        $element.animate({
            "margin-top": -55
        }, 500, function () { $element.remove(); });

        $newElement.animate({
            "margin-top": 0
        }, 500);

    };

    var counterFinished = function () {
        //---------------------------------For Vocab Assesment--------------------------//
        if ($("#Vocab").val() == "Vocab") {
            var correctAnswer = $("#correctAnswer").val();
            var radio = $("input[name=quiz]:checked").attr("value");
            var status;

            if (radio == correctAnswer) {
                status = 'true';
            }
            else {
                status = 'false';
            }

            //------Ajax Section------//
            var type = "vocab";
            $.ajax({
                url: 'GetVocabNextAssesment',
                type: "POST",
                data: { questionId: $('#questionID').val(), vocabId: $('#vocabid').val(), Status: status, Type: type, },
                success: function (data) {

                    $("#assesmentpartial").html(data);
                },
                error: function (result) {
                    $("#lblLoginerror").css("display", "block");
                }
            });
        }

        //---------------------------------For Dialog Assesment--------------------------//

        if ($("#Dialog").val() == "Dialog") {
            if ($("#QuestionType").val() == "FillBlanks") {
                var FillBlank = $("#FillBlanks").val();
                var status;
                if ($("#FillAnswerText").val() == FillBlank) {
                    status = 'true';
                }
                else {
                    status = 'false';
                }
            }

            if ($("#QuestionType").val() == "TrueFalse") {
                var TrueFalseAnswer = $("#TrueFalseAnswer").val();
                var radio = $("input[name=quiz]:checked").attr("value");
                var status;
                if (radio == TrueFalseAnswer) {
                    status = 'true';
                }
                else {
                    status = 'false';
                }
            }

            if ($("#QuestionType").val() == "Objective") {
                var OptionCorrectAnswer = $("#OptionCorrectAnswer").val();
                var radio = $("input[name=Objquiz]:checked").attr("value");
                var status;
                if (radio == OptionCorrectAnswer) {
                    status = 'true';
                }
                else {
                    status = 'false';
                }
            }
            //----------------------- Ajax Section -----------------------------------------------------//
            $.ajax({
                url: 'NextDialogAssesmentlist',
                type: "POST",
                data: { questionId: $('#questionID').val(), dialogId: $('#dialogID').val(), status: status },
                success: function (data) {
                    $("#dialogpartial").html(data);
                },
                error: function (result) {
                    $("#lblLoginerror").css("display", "block");
                }
            });
        }
        //-------------------- Ajax Section Ends -----------------------------------------------------//
    };

    var rollToEnd = function () {
        for (var itemNumber = 1; itemNumber <= 5; itemNumber++) {

            var token = "#counter_item" + itemNumber + " :first-child";
            var $element = $(token).next(); // second child

            $element.after('<div class="digit digit_cherry" style="margin-top: 55px"></div>');

            var $newElement = $element.next();
            $element.animate({
                "margin-top": -55
            }, 500, function () { $element.remove(); });

            $newElement.animate({
                "margin-top": 0
            }, 500);
        }
        $.timeout(counterFinished, 1000);
    };

    var tick = function () {
        calculateDigits();

        if (digit4 === 0) {
            if (digit3 === 0) {
                if (digit2 === 0) {
                    switchItem(1, digit1, 5);
                }
                switchItem(2, digit2, 9);
            }
            switchItem(4, digit3, 5);
        }
        switchItem(5, digit4, 9);

        decTicksLeft();

        if (getTicksLeft() === 0) {
            clearInterval(gIntervalToken);
            gIntervalToken = null;
            $.timeout(rollToEnd, 1000);
        }
    };

    window.CounterInit = function (ticksCount) {
        if (ticksCount === null || isNaN(ticksCount)) {
            ticksCount = .5 * 60;	//10 replace 2
        }
        gTicksLeft = ticksCount;
        init();
        if (gIntervalToken !== null) {
            clearInterval(gIntervalToken);
            gIntervalToken = null;
        }
        // strange chrome bug workaround
        $.timeout(function () {
            gIntervalToken = $.interval(tick, 1000);
        }, 100);
    };



})(jQuery);
