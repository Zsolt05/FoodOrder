// Frissíti az oldalt a modell alapján
function updatePage(items) {
    var productListHtml = "";
    items.forEach(function(item) {
        productListHtml += `
        <div class="card mt-3">
          <div class="card-body">
            <h5 class="card-title">${item.name}</h5>
            <p class="card-text">Ár: ${item.price}</p>
            <p class="card-text">Kategória: ${item.category.name}</p>
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
    var model = await getData("food?pageNumber="+page+"&pageSize=4");
    updatePage(model.items);
    updatePagination(model);
}

async function onLoadFoodTable()
{
    // Kezdeti oldal betöltése
    await changePage(1);
}