function getValueAtIndex(index) {
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

if (getValueAtIndex(3) === "Profile") {
    console.log("Here comes a chart");
    $.get(`/Profile/SchoolAttendanceChart/`, (response) => {
        console.log(response);
        var data = {
            labels: response[0],
            datasets: [
                    {
                        label: "Today's Attendance",
                        backgroundColor: "rgba(179,181,198,0.2)",
                        borderColor: "rgba(179,181,198,1)",
                        pointBackgroundColor: "rgba(179,181,198,1)",
                        pointBorderColor: "#fff",
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: "rgba(179,181,198,1)",
                        data: response[1]
                    },
                    {
                        label: "Yearly Average Attendance",
                        backgroundColor: "rgba(255,99,132,0.2)",
                        borderColor: "rgba(255,99,132,1)",
                        pointBackgroundColor: "rgba(255,99,132,1)",
                        pointBorderColor: "#fff",
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: "rgba(255,99,132,1)",
                        data: response[2]
                    }
                ]
        };
        //Detail Class View Chart
        const indexChart = $("#indexChart");

        var myRadarChart = new Chart(indexChart, {
            type: 'radar',
            data: data,
            options: {
                scale: {
                    reverse: false,
                    ticks: {
                        beginAtZero: true
                    }
                }
            }
        });
    });
}
