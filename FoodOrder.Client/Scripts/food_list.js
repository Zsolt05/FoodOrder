let user = JSON.parse(localStorage.getItem("data"));
let pageNum = 1;

// Frissíti az oldalt a modell alapján
function updatePage(items) {
    var productListHtml = "";
    items.forEach(function (item) {
        productListHtml += `
        <div class="card mt-3">
          <div id="${item.id}" class="card-body">
            <h5 class="card-title food-name">${item.name}</h5>
            <p class="card-text food-price">Ár: ${item.price}</p>
            <p class="card-text food-category">Kategória: ${item.category.name}</p>
            ${user.isAdmin ? `<button title="Törlés" class="btn btn-danger delete-food">X</button>` : ""}
            <button class="add-to-cart">Kosárhoz adás</button>
          </div>
        </div>`;
    });
    document.getElementById("product-list").innerHTML = productListHtml;
}

// Törlés gomb eseménykezelője
addEventListener('click', async function () {
    if (event.target.classList.contains('delete-food')) {
        if (confirm('Biztosan törölni szeretnéd?')) {
            // Törlés végrehajtása
            await deleteData(`food/${event.target.parentElement.id}`);
            alert("Sikeres törlés!");
            await changePage(pageNum);
        }
    }
});

// Frissíti a lapozó gombokat
function updatePagination(model) {
    var paginationHtml = "";
    for (var i = 1; i <= model.totalPages; i++) {
        paginationHtml += `<li class="page-item ${i === model.currentPage ? 'active' : ''}"><a class="page-link" href="#" onclick="changePage(${i})">${i}</a></li>`;
    }
    document.getElementById("pagination").innerHTML = paginationHtml;
}

// Oldalváltás eseménykezelő
async function changePage(page) {
    pageNum = page;
    var model = await getData("food?pageNumber=" + page + "&pageSize=4");
    updatePage(model.items);
    updatePagination(model);
    await initCart();
}

async function onLoadFoodTable() {
    // Kezdeti oldal betöltése
    await changePage(pageNum);
    var userLabel = document.getElementById("lblUserName");
    userLabel.innerHTML = "Bejelentkezve, mint " + user.firstName;
    if (user.isAdmin) {
        let navBar = document.getElementsByClassName("navbar-nav");
        let navItem = document.createElement("li");
        navItem.classList.add("nav-item");
        let navLink = document.createElement("a");
        navLink.classList.add("nav-link");
        navLink.href = "food.html";
        navLink.innerHTML = "Étel hozzáadása";
        navItem.appendChild(navLink);
        navBar[0].appendChild(navItem);

        let cartId = document.getElementsByClassName("cart-btn");
        cartId[0].style.display = "none";

        let buttons = document.getElementsByClassName("add-to-cart");
        for (let i = 0; i < buttons.length; i++) {
            buttons[i].innerHTML = "Módosítás";
            buttons[i].onclick = function () {
                window.location.href = "food.html?id=" + this.parentElement.id;
            }
        }
    }
}