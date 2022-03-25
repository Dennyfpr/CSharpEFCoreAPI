$.ajax({
    url: "https://swapi.dev/api/people/"
}).done((result) => {
    var content = "";
    $.each(result.results, function (idx, val) {
        content += `<tr>
                        <td>${idx+1}</td>
                        <td>${val.name}</td>
                        <td>${val.height}</td>
                        <td>${val.mass}</td>
                        <td>${val.birth_year}</td>
                        <td>${val.gender}</td>
                    </tr>`;
    })
    $('#tbSwc').html(content);
}).fail((err) => {
    console.log(err);
})