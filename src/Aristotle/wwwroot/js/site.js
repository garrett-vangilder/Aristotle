// Write your Javascript code.


function getValueAtIndex(index) {
    var str = window.location.href; 
    //return str;
    return str.split("/")[index];
}
console.log(getValueAtIndex(5));

let url = window.location.href;

if (getValueAtIndex(5)) {
    var classId = getValueAtIndex(5);
    $.get(`/Class/SchoolAttendanceChart/${classId}`, function (response) {
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
        var ctx = $("#ClassDetail");

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

}









