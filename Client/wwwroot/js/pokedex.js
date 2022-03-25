function GetPokeName() {
    let pokeNames = [];
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/?limit=60",
        async: false
    }).done((result) => {
        $.each(result.results, function (idx, val) {
            pokeNames.push(val.name);
        })
        for (var i = 0; i < pokeNames.length; i++) {
            pokeNames[i] = pokeNames[i].charAt(0).toUpperCase() + pokeNames[i].slice(1);
        }
    }).fail((err) => {
        console.log(err);
    })
    return pokeNames
}

function GetPokeIcon(idpk) {
    let r = "";
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/" + idpk,
        async: false
    }).done((result) => {
        r = result.sprites.front_default;
    }).fail((err) => {
        console.log(err);
    })
    return r
}

function GetPokeStats(idpk) {
    let r = {};
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/" + idpk,
        async: false
    }).done((result) => {
        stats = result.stats;
        r = { hp: stats[0].base_stat, atk: stats[1].base_stat, def: stats[2].base_stat, sp_atk: stats[3].base_stat, sp_def: stats[4].base_stat, spd: stats[5].base_stat};
    }).fail((err) => {
        console.log(err);
    })
    return r
}

function GetPokeTypes(idpk) {
    let r = [];
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/" + idpk,
        async: false
    }).done((result) => {
        types = result.types;
        for (var i = 0; i < types.length; i++) {
            r.push(types[i].type.name.charAt(0).toUpperCase() + types[i].type.name.slice(1));
        }
    }).fail((err) => {
        console.log(err);
    })
    return r
}

function GetPokePhysic(idpk) {
    let r = {};
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/" + idpk,
        async: false
    }).done((result) => {
        r = { ht: result.height*10, wt: result.weight/10 };
    }).fail((err) => {
        console.log(err);
    })
    return r
}

function GetPokeAbility(idpk) {
    let r = "";
    $.ajax({
        url: "https://pokeapi.co/api/v2/pokemon/" + idpk,
        async: false
    }).done((result) => {
        r = result.abilities[0].ability.name.charAt(0).toUpperCase() + result.abilities[0].ability.name.slice(1);
    }).fail((err) => {
        console.log(err);
    })
    return r
}

$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/?limit=60"
}).done((result) => {

    let pokeNames = GetPokeName();
    let pokeList = "";
    let pokeDetails = "";

    // Get Data in HTML
    $.each(result.results, function (idx, val) {
        // Get each data from API
        pokeId = idx + 1;
        pokeTypes = GetPokeTypes(pokeId);
        pokeStats = GetPokeStats(pokeId);
        pokePhysic = GetPokePhysic(pokeId);
        pokeAbility = GetPokeAbility(pokeId);
        pokeIconUrl = GetPokeIcon(pokeId);

        // Colorized Pokemon Types Tags
        pokeTypesTxt = "";
        for (var i = 0; i < pokeTypes.length; i++) {
            if (pokeTypes[i] == "Grass" || pokeTypes[i] == "Bug") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-success">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Fire" || pokeTypes[i] == "Fairy") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-danger">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Electric" || pokeTypes[i] == "Ground" || pokeTypes[i] == "Dragon" || pokeTypes[i] == "Fighting") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-warning">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Water") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-primary">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Flying" || pokeTypes[i] == "Ice") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-info">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Poison" || pokeTypes[i] == "Dark" || pokeTypes[i] == "Psychic" || pokeTypes[i] == "Ghost") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-dark">${pokeTypes[i]}</button></span> `;
            } else if (pokeTypes[i] == "Normal" || pokeTypes[i] == "Rock" || pokeTypes[i] == "Steel") {
                pokeTypesTxt += `<span><button type="button" class="btn btn-secondary">${pokeTypes[i]}</button></span> `;
            } else {
                pokeTypesTxt += `<span><button type="button" class="btn btn-light">${pokeTypes[i]}</button></span> `;
            }
        }

        // Colorized Pokemon Stats Bar
        pokeStatsBar = "";
        for (const [key, value] of Object.entries(pokeStats)) {
            if (value >= 90) {
                pokeStatsBar += `<div class="progress mt-1 mb-4"> <div class="progress-bar bg-success" role="progressbar" style="width: ${value}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${value}</div> </div> `;
            } else if (value >= 60)  {
                pokeStatsBar += `<div class="progress mt-1 mb-4"> <div class="progress-bar bg-info" role="progressbar" style="width: ${value}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${value}</div> </div> `;
            } else if (value >= 30)  {
                pokeStatsBar += `<div class="progress mt-1 mb-4"> <div class="progress-bar bg-warning" role="progressbar" style="width: ${value}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${value}</div> </div> `;
            } else if (value > 0)  {
                pokeStatsBar += `<div class="progress mt-1 mb-4"> <div class="progress-bar bg-danger" role="progressbar" style="width: ${value}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${value}</div> </div> `;
            } else {
                pokeStatsBar += `<div class="progress mt-1 mb-4"> <div class="progress-bar" role="progressbar" style="width: 0%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div> </div> `;
            }
        }

        // Add data to HTML
        pokeList += `<tr>
                        <td class="text-center align-middle">${pokeId}</td>
                        <td class="text-center"><img src="${pokeIconUrl}" class="rounded mx-auto d-block" alt=""></td>
                        <td class="align-middle">${pokeNames[idx]}</td>
                        <td class="text-center align-middle"><button type="button" class="btn btn-light" data-toggle="modal" data-target="#ModalPoke${pokeId}">View Details</button></td>
                     </tr>`;
        pokeDetails += `<div class="modal bd-example-modal-lg" id="ModalPoke${pokeId}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Pokemon Details</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body" id="DetailPoke">
                                    <div class="container mt-3 mb-3">
                                        <div class="row">
                                            <div class="col-4">
                                                <h4 class="text-center font-weight-bold mb-0">${pokeNames[idx]}</h4>
                                                <p class="text-center mb-0"><img src="https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/${pokeId}.png" class="rounded mt-5 mb-4" width="200" height="200" alt=""></p>                                                
                                                <p class="text-center mb-0">${pokeTypesTxt}</p>
                                            </div>                                        
                                            <div class="col align-self-end">
                                                <div class="row mb-2">
                                                    <div class="col">
                                                        <p class="font-weight-bold mb-1">Height </p>
                                                        <p class="b-0">${pokePhysic.ht} cm</p>
                                                    </div>
                                                    <div class="col">
                                                        <p class="font-weight-bold mb-1">Weight </p>
                                                        <p class="mb-0">${pokePhysic.wt} kg</p>
                                                    </div>
                                                    <div class="col">
                                                        <p class="font-weight-bold mb-1">Ability </p>
                                                        <p class="mb-0">${pokeAbility}</p>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-3">
                                                        <p class="font-weight-bold">Hit Points  </p>
                                                        <p class="font-weight-bold">Attack  </p>
                                                        <p class="font-weight-bold">Defense  </p>
                                                        <p class="font-weight-bold">Sp Attack  </p>
                                                        <p class="font-weight-bold">Sp Defense  </p>
                                                        <p class="font-weight-bold mb-0">Speed  </p>
                                                    </div>
                                                    <div class="col pl-0">
                                                        ${pokeStatsBar}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                              </div>
                          </div>
                      </div>`;
    })

    // Put Data in HTML
    $('#tbSwc').html(pokeList);
    $('#pokeDets').html(pokeDetails);


}).fail((err) => {
    console.log(err);
})