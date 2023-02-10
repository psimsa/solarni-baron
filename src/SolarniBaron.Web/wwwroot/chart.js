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

// Function to highlight item X in the chart
function highlightItem(chartId, index) {
  console.log(`highlighting item ${index} in chart ${chartId}`);
  var canvas = document.getElementById(chartId);
  debugger;
  if (canvas === null) return;
  var ctx = canvas.getContext('2d');
  var chart = ctx.chart;
  if (chart === undefined) return;
  var dataset = chart.data.datasets[0];
  dataset.forEach(d => pointRadius[d] = 3);
  dataset.pointRadius[index] = 6;
  chart.update();
}
