$(document).ready(function () {
    var $questionsContainer = $("#questionsContainer");
    var $addQuestion = $("#addQuestion");
    var questionCounter = document.querySelectorAll(".question").length;

    function UpdateQuestionAttributesIndexes() {
        $(".question").each(function (index, question) {
            $(question).find("textarea").attr('id', function (_, idAttr) {
                return idAttr.replace(/Questions_\d+__/, 'Questions_' + index + '__');
            });

            $(question).find("textarea").attr('name', function (_, nameAttr) {
                return nameAttr.replace(/Questions\[\d+\]/, 'Questions[' + index + ']');
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
        clone.find('input').val('');
        clone.find('input[type=hidden]').val('');
        clone.find('span.question-title').text(`Question ${questionCounter + 1}`)
       
        $questionsContainer.append(clone);
        questionCounter++;
    });

    $questionsContainer.on("click", ".delete-question", function () {
        if (questionCounter > 1) {
            $(this).closest('.question').remove();
            UpdateQuestionAttributesIndexes();
            questionCounter--;
        }
    });
});