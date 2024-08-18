$(document).ready(function () {
    var closeErrorsButton = document.getElementById('closeErrorsButton');
    var spaceBottom = document.getElementById('spaceBeforeErrorModal');
    closeErrorsButton.addEventListener('click', function () {
        spaceBottom.style.display = 'none';
    });
});