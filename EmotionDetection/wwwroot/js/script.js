function revealSurprise() {
    const giftBox = document.querySelector('.gift-box');
    const surprise = document.getElementById('surprise');


    giftBox.style.display = 'none';


    surprise.classList.remove('hidden');


    confetti({
        particleCount: 100,
        spread: 70,
        origin: { y: 0.6 }
    });
}



$(document).ready(function () {

    $("#mediumModal").on('shown.bs.modal', function () {
        console.log("Medium Modal açıldı");
    });


    $("#mediumModal").on('hidden.bs.modal', function () {
        console.log("Medium Modal kapandı");
    });
});


