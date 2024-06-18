window.drawRectangle = function () {
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    ctx.fillStyle = 'green';
    ctx.fillRect(10, 10, 150, 100);
}

window.drawCircle = function () {
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    ctx.beginPath();
    ctx.arc(200, 150, 50, 0, 2 * Math.PI);
    ctx.fillStyle = 'blue';
    ctx.fill();
}

window.clearCanvas = function () {
    var canvas = document.getElementById('myCanvas');
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}