function expandTextArea(element, button) {
    if (element.style.height == 'auto') {
        element.style.height = '7.2rem';
        button.textContent = 'Ler Mais'
    } else {
        element.style.height = 'auto';
        button.textContent = 'Mostrar Menos'
    }


}

function expandCapSection(elementId, button) {
    element = document.getElementById(elementId)
    if (element.style.overflowY != 'visible') {
        element.style.overflowY = 'visible';
        button.textContent = 'Ver Menos'
        element.style.height = 'auto'
    } else {
        element.style.overflowY = 'hidden';
        button.textContent = 'Ver Mais'
        element.style.height = '14.6rem'
    }
}

const textArea = document.getElementById('textArea');
const btnDescription = document.getElementById('buttonDescription');
const chapSection = document.getElementById('chapters');
const btnChapter = document.getElementById('buttonChapter');
const bioText = document.getElementById('textBio');
const lenghtBio = document.getElementById('lenghtBio');
const phoneText = document.getElementById('phoneText')

var Dbid;

if (btnDescription) {
    btnDescription.addEventListener('click', function () {
        expandTextArea(textArea, btnDescription)
    });

    btnChapter.addEventListener('click', function () {
        expandCapSection(chapSection, btnChapter);
    });
}

function CloseModal(id) {
    let modal = document.getElementById(id)
    modal.style.top = modal.offsetTop == "-160" ? "5rem" : "-10rem";
}

const unPublishFunc = function () { ConfirmFunc('confirmUnPublish', 'bookConfig') }
const PublishFunc = function () { ConfirmFunc('confirmPublish', 'bookConfig') }
function SaveButton(id, blur, dbId) {
    Dbid = dbId;
    let save = document.getElementById(id);
    let status = document.getElementById('status').value;
    let publishButton = document.getElementById('publishButton');
    save.style.display = save.style.display == 'flex' ? 'none' : 'flex'
    publishButton.removeEventListener('click', unPublishFunc)
    publishButton.removeEventListener('click', PublishFunc)
    if (status == 1) {

        publishButton.addEventListener('click',unPublishFunc) ;
        publishButton.getElementsByTagName('span')[0].textContent = 'DesPublicar';
    } else {
        publishButton.addEventListener('click',PublishFunc);
        publishButton.getElementsByTagName('span')[0].textContent = 'Publicar';
    }
    if (blur) {
        let element = document.getElementById('center');
        element.classList.toggle('filterBlur');

    }
}

function ToggleIntarfaceId(id, dbId)
{
    Dbid = dbId;
    let save = document.getElementById(id);
    save.style.display = save.style.display == 'flex' ? 'none' : 'flex'
}

function DeleteDbById() {
    let content = document.getElementById("contentBooks");
    document.getElementById("confirmRemove").style.display = "none";
    let element = document.getElementById('center');
    element.classList.toggle('filterBlur');
    $.ajax({
        type: "POST",
        url: "book/DeleteByIdBook",
        data: { bookId : Dbid },

        success: function (response) {
            content.innerHTML = response;
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function PublishBook() {
    document.getElementById("confirmPublish").style.display = "none";
    let element = document.getElementById('center');
    element.classList.toggle('filterBlur');
    $.ajax({
        type: "POST",
        url: "book/PublishBook",
        data: { bookId: Dbid },

        success: function (response) {
            ChangeContent(1);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function UnPublishBook() {

    document.getElementById("confirmUnPublish").style.display = "none";
    let element = document.getElementById('center');
    element.classList.toggle('filterBlur');
    $.ajax({
        type: "POST",
        url: "book/UnPublishBook",
        data: { bookId: Dbid },

        success: function (response) {
            ChangeContent(2);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

function ConfirmFunc(id, idMenu) {
    let deleteMenu = document.getElementById(id);
    let configs = document.getElementById(idMenu);
    configs.style.display = configs.style.display == 'flex' ? 'none' : 'flex';
    deleteMenu.style.display = deleteMenu.style.display == 'block' ? 'none' : 'block';
}

function ToggleVisible(id,dbid) {
    Dbid = dbid
    let save = document.getElementById(id);
    let element = document.getElementById('center');
    save.style.display = save.style.display == 'flex' ? 'none' : 'flex'
    element.classList.toggle('filterBlur');
}

function ToggleVisibleNoBlur(id) {
    let save = document.getElementById(id);
    save.style.display = save.style.display == 'flex' ? 'none' : 'flex'
}

function ShowAllCaps(id,idBook,idContent) {
    let save = document.getElementById(id);
    let allCaps = document.getElementById('allcaps');
    save.style.display = save.style.display == 'flex' ? 'none' : 'flex'

    $.ajax({
        
        type: "POST",
        url: "/book/GetAllCaps",
        data: {
            bookId: idBook,
            contentId: idContent
        },

        success: function (response) {
            allCaps.innerHTML = response;
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    })
}

function DeleteByIdCap() {
    $.ajax({
        type: "POST",
        url: "/book/RemoveChap",
        data: { bookId: Dbid },

        success: function (response) {
            location.reload()
        },
        error: function (xhr, status, error) {
            alert('Error Interno');
        }
    });
}

if (bioText) {
    lenghtBio.textContent = bioText.value.length + "/300";
    bioText.addEventListener('input', function (event) {
        lenghtBio.textContent = bioText.value.length + "/300";
        if (bioText.value.length >= 300) {
            bioText.value = bioText.value.substring(0, 299);
        }
    });

    phoneText.addEventListener('input', function (event) {
        if (this.value.length > 11 && /^\d+$/.test(this.value)) {
            this.value = this.value.substring(0, 11);
        }
        this.value = this.value.replace(/^(\d{2})(\d{5})(\d{4})$/, "($1) $2-$3");
        this.value = this.value.replace(/[a-zA-Z]/g, '');

    });
}

function AddChap(id) {
    $.ajax({
        type: "POST",
        url: "/book/AddChap",
        data: { BookId: id },

        success: function (response) {
            location.reload()
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

const inputFile = document.querySelector("#arquivo");
const pictureImage = document.querySelector("#imageUser");

if (inputFile) {
    inputFile.addEventListener("change", function (e) {
        const inputTarget = e.target;
        const file = inputTarget.files[0];

        if (file) {
            const reader = new FileReader();

            reader.addEventListener("load", function (e) {
                const readerTarget = e.target;

                pictureImage.src = readerTarget.result;
                inputFile.querySelector('p').textContent = '';
                inputFile.style.alignSelf = "end";
                inputFile.style.backgroundColor = 'rgb(30,30,30,0.4)';
                inputFile.style.padding = '10px'
            });

            reader.readAsDataURL(file);
        } else {
            pictureImage.innerHTML = pictureImageTxt;
            inputFile.querySelector('p').textContent = 'Selecione uma Imagem';
            inputFile.style.alignSelf = "center";
            inputFile.style.backgroundColor = 'transparent';

        }
    });
}
function expandCap(elementId, button) {
    let element = document.getElementById(elementId);
    if (element.style.overflowY == 'hidden') {
        element.style.overflowY = 'visible';
        button.textContent = 'Mostrar Menos'
        element.style.height = 'auto'
    } else {
        element.style.overflowY = 'hidden';
        button.textContent = 'Mostrar Mais'
        element.style.height = '16.4rem';
    }

}


function writeComment() {
    let idContent = document.getElementById('idContent').value;
    let commentText = document.getElementById('comment').value;
    if (commentText.length > 0) {
        let commentDTO = {
            Mensagem: commentText,
            BookContentId: parseInt(idContent)
        }    
        $.ajax({
            type: "POST",
            url: "/book/PublishComment",
            data: { comment: commentDTO },

            success: function (response) {
                if (response == "Login") {
                    alert("Você não está Logado")
                } else {
                    location.reload()
                }
                
            },
            error: function (xhr, status, error) {
                alert.error(error);
            }
        }); 
    }
    
}

function replyComment(id,idComment) {
    let commentText = document.getElementById(id).value;
    let idContent = document.getElementById('idContent').value;
    if (commentText.length > 0 && commentText != null) {
        let replyDTO = {
            Mensagem: commentText,
            CommentId: parseInt(idComment),
            BookContentId: parseInt(idContent)
        }
        
        $.ajax({
            type: "POST",
            url: "/book/PublishComment",
            data: { comment: replyDTO },

            success: function (response) {
                location.reload()
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }

}

function openResponses(id, button) {
    let replysContent = document.getElementById(id);
    if (replysContent.style.display == 'flex') {
        replysContent.style.display = 'none'
        let quant = replysContent.getElementsByTagName('li').length;
        button.innerHTML = `<i class="fa-solid fa-caret-down"></i> Mostrar Respostas(${quant})`;
    } else {
        replysContent.style.display = 'flex'
        let quant = replysContent.getElementsByTagName('li').length;
        button.innerHTML = `<i class="fa-solid fa-caret-up"></i> Ocultar Respostas(${quant})`;
    }
}


function SearchBook(id) {
    searchField = document.getElementById(id);
    searchValue = searchField.value != '' ? searchField.value : '\u0020';
    window.location.href = `${window.location.origin}/Pesquisa/${searchValue}`;
}
try {
    ChangeContent(1);
}
catch (error) {
    console.log(error);
}




