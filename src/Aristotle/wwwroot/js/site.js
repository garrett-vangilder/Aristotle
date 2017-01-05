﻿function getValueAtIndex(index) {
    let str = window.location.href; 
    return str.split("/")[index];
}

if (getValueAtIndex(5)) {
    var classId = getValueAtIndex(5);
    $.get(`/Class/SchoolAttendanceChart/${classId}`,  (response) => {
        console.log(response);
        var data = {
            labels: response[0],
            datasets: [
                {
                    label: "Class Attendance",
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
        const ctx = $("#ClassDetail");

        let myLineChart = Chart.Line(ctx, {
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
    });
}
