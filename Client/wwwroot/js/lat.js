var text = document.getElementById("text");
var bt1 = document.getElementById("bt1");

bt1.addEventListener('click', function () {
    var vis = text.style.visibility;
    if (vis == 'hidden') {
        text.style.visibility = 'visible';
    } else {
        text.style.visibility = 'hidden';
    }
});

const animals = [
    { name: "Fluffy", species: "cat", class: { name: "mamalia" } },
    { name: "Nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "Garfield", species: "cat", class: { name: "mamalia" } },
    { name: "Dory", species: "fish", class: { name: "invertebrata" } },
    { name: "Camello", species: "cat", class: { name: "mamalia" } },
]

// Solusi Soal 1
const OnlyCat = [];
for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == "cat") {
        OnlyCat.push(animals[i]);
    }
}

// Solusi Soal 2
for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name = "Non-Mamalia"
    }
}