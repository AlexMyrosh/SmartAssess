$(document).ready(function () {
    var $questionsContainer = $("#questionsContainer");
    var $addQuestion = $("#addQuestion");
    var questionCounter = document.querySelectorAll("#questionsContainer div").length;

    function UpdateQuestionAttributesIndexes() {
        $(".question").each(function (index, question) {

            $(question).find("label").attr('for', function (_, forAttr) {
                return forAttr.replace(/Questions_\d+__/, 'Questions_' + index + '__');
            });

            $(question).find("textarea").attr('id', function (_, idAttr) {
                return idAttr.replace(/Questions_\d+__/, 'Questions_' + index + '__');
            });

            $(question).find("textarea").attr('name', function (_, nameAttr) {
                return nameAttr.replace(/Questions\[\d+\]/, 'Questions[' + index + ']');
            });

            $(question).find("span").attr('data-valmsg-for', function (_, valmsgforAttr) {
                return valmsgforAttr.replace(/Questions\[\d+\]/, 'Questions[' + index + ']');
            });

            $(question).find("input").attr('id', function (_, idAttr) {
                return idAttr.replace(/Questions_\d+__/, 'Questions_' + index + '__');
            });

            $(question).find("input").attr('name', function (_, nameAttr) {
                return nameAttr.replace(/Questions\[\d+\]/, 'Questions[' + index + ']');
            });
        });
    }

    $addQuestion.click(function () {
        var clone = $(".question:first").clone();

        // Updates indexes to map fields to model properties
        clone.html(
            clone.html().replace(/\[0\]/g, '[' + questionCounter + ']').replace(/_0__/g, '_' + questionCounter + '__')
        );

        // In case if first question has some text, then added question field should be empty
        clone.find('textarea').val('');
        clone.find('input[type=hidden]').val('');

        console.log(clone.html());

        $questionsContainer.append(clone);
        questionCounter++;
    });

    $questionsContainer.on("click", ".delete-question", function () {
        if ($questionsContainer.children().length > 1) {
            $(this).closest('.question').remove();
            UpdateQuestionAttributesIndexes();
            questionCounter--;
        }
    });
});