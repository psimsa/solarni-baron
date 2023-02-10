window.setup = (id, config) => {
  var canvas = document.getElementById(id);
  var ctx = canvas.getContext('2d');
  var c = new Chart(ctx, config);
  window.addEventListener("resize", function () {
    // console.log('resizing');
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;
    c.resize();
  });
}
