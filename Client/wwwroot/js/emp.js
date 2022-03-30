$(function () {
    // INITIALIZE DATEPICKER PLUGIN
    $('.datepicker').datepicker({
        clearBtn: true,
        format: "yyyy-mm-dd"
    });
});

$.ajax({
    url: "https://localhost:44383/API/universities/",
    type: "GET",
    data: {},
}).done((result) => {
    console.log(result[0].name);
    let listUni = '<option selected value="">Pilih ...</option>';
    $.each(result, function (idx, val) {
        listUni += `<option value="${val.id}">${val.name}</option>`;
    });
    $("#InputUn").html(listUni);
}).fail((error) => {
    console.log(error);
});

$.ajax({
    url: "https://localhost:44383/API/employees/",
    type: "GET",
    data: {}
}).done((result) => {
    let editMod = ``;
    $.each(result, function (idx, val) {
        gdName = '';
        if (val.gender == 0) {
            gdName = 'Laki-Laki';
        } else {
            gdName = 'Perempuan';
        }
        editMod +=
        `
            <div class="modal bd-example-modal-lg" id="ModalEmpEdit${idx + 1}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">${val.nik} - ${val.firstName} ${val.lastName}</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <form id="editDataForm${idx + 1}" class="needs-validation" novalidate>
                                <div class="row">
                                    <div class="form-group col">
                                        <label for="InputFN">NIK</label>
                                        <input type="text" class="form-control" id="NIK${idx + 1}" aria-describedby="inputFN" placeholder="${val.nik}" disabled>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col">
                                        <label for="InputFN">Nama Depan</label>
                                        <input type="text" class="form-control" id="FN${idx + 1}" aria-describedby="inputFN" placeholder="${val.firstName}" disabled>
                                    </div>
                                    <div class="form-group col">
                                        <label for="InputLN">Nama Belakang</label>
                                        <input type="text" class="form-control" id="LN${idx + 1}" aria-describedby="inputLN" placeholder="${val.lastName}" disabled>
                                    </div>
                                    <div class="form-group col">
                                        <label for="InputGd">Jenis Kelamin</label>
                                        <select class="custom-select mr-sm-2" id="Gd${idx + 1}" disabled>
                                            <option selected>${gdName}</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group col">
                                        <label for="InputEm">Email</label>
                                        <input type="text" class="form-control" id="Em${idx + 1}" aria-describedby="inputEm" placeholder="${val.email}" disabled>
                                    </div>
                                    <div class="form-group col">
                                        <label for="InputPh">Nomor Telepon</label>
                                        <input type="text" class="form-control" id="Ph${idx + 1}" aria-describedby="inputPh" placeholder="Nomor Telepon" value="${val.phone}" required>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="InputSl">Gaji</label>
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">$</span>
                                        <input type="text" class="form-control" id="Sl${idx + 1}" aria-describedby="inputSl" placeholder="${val.salary}" value="${val.salary}" required>
                                    </div>
                                </div>
                                <br />
                                <button type="submit" id="dataSubmit${idx + 1}" class="btn btn-primary rounded float-right" style="width: 150px">Submit</button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;
    });
    $("#empEdit").html(editMod);

    $.each(result, function (idx, val) {
        window[`dS${idx + 1}`] = document.getElementById(`dataSubmit${idx + 1}`);
        window[`dS${idx + 1}`].addEventListener('click', function () {
            var obj = new Object();
            obj.nik = val.nik;
            obj.firstName = val.firstName;
            obj.lastName = val.lastName;
            obj.phone = $(`#Ph${idx + 1}`).val();
            obj.birthDate = val.birthDate;
            obj.salary = parseInt($(`#Sl${idx + 1}`).val());
            obj.email = val.email;
            obj.gender = val.gender;
            console.log(obj);
            let data = JSON.stringify(obj);
            $(`#editDataForm${idx+1}`).validate({ errorPlacement: function (error, element) { } });

            if ($(`#editDataForm${idx + 1}`).valid()) {
                $.ajax({
                    url: "https://localhost:44392/employees/put/",
                    type: "PUT",
                    data: data,
                    contentType: "application/json"
                }).done((result) => {
                    Swal.fire(
                        'Aksi Berhasil!',
                        'Data telah diperbaharui pada basis data!',
                        'success'
                    );
                    console.log(result);
                }).fail((error) => {
                    Swal.fire(
                        'Aksi Gagal!',
                        'Data tidak diperbaharui pada basis data!',
                        'error'
                    );
                    console.log(error);
                });
            }
        });
    });

    let delMod = '';
    $.each(result, function (idx, val) {
        delMod +=
        `
        <div class="modal" tabindex="-1" role="dialog" id="ModalEmpDel${idx+1}">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Delete Confirmation</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete the data from database?</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-light" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-danger" id="dataDelete${idx+1}">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
        `;
    });
    $("#empDel").html(delMod);

    $.each(result, function (idx, val) {
        window[`dD${idx + 1}`] = document.getElementById(`dataDelete${idx + 1}`);
        window[`dD${idx + 1}`].addEventListener('click', function () {
            $.ajax({
                url: "https://localhost:44392/employees/delete/" + val.nik,
                type: "DELETE",
                data: {},
                contentType: "application/json"
            }).done((result) => {
                Swal.fire(
                    'Aksi Berhasil!',
                    'Data telah dihapus pada basis data!',
                    'success'
                );
                console.log(result);
            }).fail((error) => {
                Swal.fire(
                    'Aksi Gagal!',
                    'Data tidak dihapus pada basis data!',
                    'error'
                );
                console.log(error);
            });
        });
    });
}).fail((error) => {
    console.log(error);
});

var dS = document.getElementById(`dataSubmit`);
dS.addEventListener('click', function () {
    var obj = new Object();
    obj.firstName = $("#InputFN").val();
    obj.lastName = $("#InputLN").val();
    obj.phone = $("#InputPh").val();
    obj.birthDate = $("#InputBd").val();
    obj.salary = parseInt($("#InputSl").val());
    obj.email = $("#InputEm").val();
    obj.gender = parseInt($("#InputGd :selected").val());
    obj.password = $("#InputPw").val();
    obj.id = 0;
    obj.degree = $("#InputDg").val();
    obj.gpa = $("#InputGp").val();
    obj.university_id = parseInt($("#InputUn").val());

    let data = JSON.stringify(obj);

    $("#addDataForm").validate({ errorPlacement: function (error, element) { }});

    if ($("#addDataForm").valid()) {
        $.ajax({
            url: "https://localhost:44392/employees/createnewprofile",
            type: "POST",
            data: data,
            contentType: "application/json"
        }).done((result) => {
            Swal.fire(
                'Aksi Berhasil!',
                'Data telah ditambahkan pada basis data!',
                'success'
            );
            console.log(result);
        }).fail((error) => {
            Swal.fire(
                'Aksi Gagal!',
                'Data tidak ditambahkan pada basis data!',
                'error'
            );
            console.log(error);
        });
    }
});

$(document).ready(function () {
    let tb = $('#tbEmp').DataTable({
        ajax: {
            url: 'https://localhost:44392/employees/getallprofile',
            dataSrc: ''
        },
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Add New Data',
                attr: {
                    'data-toggle': 'modal',
                    'data-target': '#ModalAddNew'
                },
                action: function (e, dt, node, config) {
                    
                }
            },
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'csvHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                }
            }
        ],
        columnDefs: [
            { orderable: false, searchable: false, targets: 0}
        ],
        columns: [
            { data: null, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1 } },
            { data: 'nik' },
            { data: 'fullName' },
            { data: 'gender' },
            { data: 'email' },
            { data: 'phone' },
            { data: 'salary', render: function (data, type, row) { return '$' + data } },
            { data: 'gpa' },
            { data: 'degree' },
            { data: 'universityName' },
            { data: null, render: function (data, type, row, meta) { return `<p class="text-center mb-0"><button type="button" class="btn btn-warning btn-sm" data-toggle="modal" data-target="#ModalEmpEdit${meta.row + meta.settings._iDisplayStart + 1}">Edit</button> <button type="button" class="btn btn-danger btn-sm" data-toggle="modal" data-target="#ModalEmpDel${meta.row + meta.settings._iDisplayStart + 1}">X</button></p>`; } }
        ]
    });
    tb.on('order.dt search.dt', function () {
        tb.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();