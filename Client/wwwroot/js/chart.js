$.ajax({
    url: "https://localhost:44383/API/employees/",
    type: "GET",
    data: {},
}).done((result) => {
    let male = 0;
    let female = 0;
    let total = 0;
    $.each(result, function (idx, val) {
        if (val.gender == 0) {
            male++;
        } else {
            female++;
        }
        total++;
    });

    let gdCont = {
        chart: {
            type: 'pie'
        },
        series: [male, female],
        labels: ['Laki-laki', 'Perempuan']
    }
    let gdChart = new ApexCharts(document.querySelector("#piechart"), gdCont);
    gdChart.render();
}).fail((error) => {
    console.log(error);
});

$.ajax({
    url: "https://localhost:44383/API/employees/masterdata",
    type: "GET",
    data: {},
}).done((result) => {
    console.log(result);

    let univCont = {
        chart: {
            type: 'bar'
        },
        series: [{
            name: 'Universitas Asal',
            data: [30, 40, 20]
        }],
        xaxis: {
            categories: ['UI', 'ITB', 'UGM']
        }
    }
    let univChart = new ApexCharts(document.querySelector("#barchart"), univCont);
    univChart.render();
}).fail((error) => {
    console.log(error);
});