$.ajax({
    url: "https://localhost:44383/API/employees/",
    type: "GET",
    data: {},
}).done((result) => {
    console.log(result);

}).fail((error) => {
    console.log(error);
});

let options1 = {
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

let chart1 = new ApexCharts(document.querySelector("#barchart"), options1);

chart1.render();

let options2 = {
    chart: {
        type: 'pie'
    },
    series: [50, 30, 15, 15],
    labels: ['Umur 20s', 'Umur 30s', 'Umur 40s', 'Umur 50s']
}

let chart2 = new ApexCharts(document.querySelector("#piechart"), options2);

chart2.render();