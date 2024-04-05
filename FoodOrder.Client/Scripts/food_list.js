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
            <button class="add-to-cart">Kosárhoz adás</button>
          </div>
        </div>`;
    });
    document.getElementById("product-list").innerHTML = productListHtml;
}

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
    var model = await getData("food?pageNumber=" + page + "&pageSize=4");
    updatePage(model.items);
    updatePagination(model);
    await initCart();
}

async function onLoadFoodTable() {
    // Kezdeti oldal betöltése
    await changePage(1);
    var userLabel = document.getElementById("lblUserName");
    var user = JSON.parse(localStorage.getItem("data"));
    userLabel.innerHTML = "Bejelentkezve, mint " + user.firstName;
}