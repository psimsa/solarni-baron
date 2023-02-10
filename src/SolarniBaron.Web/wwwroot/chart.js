window.setup = (id, config) => {
  var canvas = document.getElementById(id);
  var ctx = canvas.getContext('2d');
  var c = new Chart(ctx, config);
}

// Function to highlight item X in the chart
function update(id, config) {
  var chart = Chart.getChart(id)
  if (chart === undefined) return;
  console.log("updating chart");
  chart.data.datasets[0].data = config.data.datasets[0].data;
  chart.data.labels = config.data.labels;
  chart.update();
}
