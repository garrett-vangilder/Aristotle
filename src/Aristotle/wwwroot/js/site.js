// Write your Javascript code.

$.get("/Class/SchoolAttendanceChart", function (response) {
    console.log(response);
    var data = {
        labels: response[0],
        datasets: [
            {
                label: "School Attendance",
                fill: false,
                lineTension: 0.1,
                backgroundColor: "rgba(75,192,192,0.4)",
                borderColor: "rgba(75,192,192,1)",
                borderCapStyle: 'butt',
                borderDash: [],
                borderDashOffset: 0.0,
                borderJoinStyle: 'miter',
                pointBorderColor: "rgba(75,192,192,1)",
                pointBackgroundColor: "#fff",
                pointBorderWidth: 1,
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(75,192,192,1)",
                pointHoverBorderColor: "rgba(220,220,220,1)",
                pointHoverBorderWidth: 2,
                pointRadius: 1,
                pointHitRadius: 10,
                data: response[1],
                spanGaps: true,
            }
        ]
    };
    //Detail Class View Chart
    var ctx = $("#ClassDetail");
    console.dir(ctx);

    var myLineChart = Chart.Line(ctx, {
        data: data,
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });

    return data;
});



