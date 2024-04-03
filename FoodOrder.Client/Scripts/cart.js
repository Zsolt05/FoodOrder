// kosar-nyitva
let cart = document.querySelector("#cart");
let cartModalOverlay = document.querySelector('.cart-modal-overlay');

cart.addEventListener('click', function () {
    var cartQuantity = document.getElementsByClassName('cart-quantity')[0]
    if (cartQuantity.textContent === '0') {
        alert('A kosár üres!');
        return;
    }
    if (cartModalOverlay.style.transform === 'translateX(-200%)') {
        cartModalOverlay.style.transform = 'translateX(0)';
    } else {
        cartModalOverlay.style.transform = 'translateX(-200%)';
    }
})
// kosar-nyitva vege

// kosar-zarva
const closeBtn = document.querySelector('#close-btn');

closeBtn.addEventListener('click', () => {
    cartModalOverlay.style.transform = 'translateX(-200%)';
});

cartModalOverlay.addEventListener('click', (e) => {
    if (e.target.classList.contains('cart-modal-overlay')) {
        cartModalOverlay.style.transform = 'translateX(-200%)'
    }
})
// kosar-zarva vege

// kosarhoz-adas
async function initCart() {
    const addToCartBtns = document.getElementsByClassName('add-to-cart');
    for (let i = 0; i < addToCartBtns.length; i++) {
        let button = addToCartBtns[i];
        button.addEventListener('click', addToCart)
    }
    await loadCart();
}

async function addToCart(event) {
    let button = event.target;
    var cartItem = button.parentElement;
    await addItemToCart(cartItem.id);
}

async function addItemToCart(id) {
    var result = await postData(`cart/add?productId=${id}&quantity=1`);
    if (result.Message) {
        alert(result.Message);
        return;
    }
    await loadCart();
}

// kosarhoz-adas vege

// kosar-tartalma

async function loadCart() {
    let result = await getData("cart?pageSize=10");
    if (result.Message) {
        alert(result.Message);
        return;
    }
    var foods = result.items;
    clearCart();
    var cartQuantity = document.getElementsByClassName('cart-quantity')[0];
    cartQuantity.textContent = foods?.length ?? 0;
    var cartRows = document.getElementsByClassName('cart-rows')[0];
    var total = 0;
	if(!foods)
	{
		return;
	}
    for (const food of foods) {
        total = total + (food.price * food.quantity)
        var cartRow = document.createElement('div');
        cartRow.classList.add('cart-row');
        var cartRowItems =
        `
        <div class="cart-row">
            <span class ="food-name">Étel neve: ${food.foodName}</span>
            <span class ="food-info">Egységár: ${food.price} Ft | Mennyiség: ${food.quantity}</span>
            <button class="btn btn-danger remove-btn" onclick="removeItem(${food.foodId})">Törlés</button>
        </div>
        `//a gombot ki lehetne húzni szélre
        cartRow.innerHTML = cartRowItems;
        cartRows.append(cartRow);
    }
    document.getElementsByClassName('total-price')[0].innerText = total + ' Ft'
}

// kosar-tartalma vege

// eltavolitas-a-kosarbol

async function removeItem(id) {
    let result = await postData(`cart/remove?productId=${id}`);
    if (result.Message) {
        alert(result.Message);
        return;
    }
    await loadCart();
}

// eltavolitas-a-kosarbol vege

// fizetes
const purchaseBtn = document.querySelector('#purchase-btn');

purchaseBtn.addEventListener('click', purchaseBtnClicked)

async function purchaseBtnClicked() {
    if (document.getElementsByClassName('cart-quantity')[0].textContent === 0) {
        alert('A kosár üres!');
        return;
    }
    let result = await postData('cart/finish', {});
    if (result.Message) {
        alert(result.Message);
        return;
    }
    alert('Köszönjük a rendelést!');
    clearCart();
    cartModalOverlay.style.transform = 'translateX(-100%)';
}

// fizetes-vege

// kosar-uritese

function clearCart() {
    var cartRows = document.getElementsByClassName('cart-rows')[0];
    while (cartRows.childNodes.length > 0) {
        cartRows.childNodes[0].remove();
    }
    var cartQuantity = document.getElementsByClassName('cart-quantity')[0];
    cartQuantity.textContent = 0;
}

// kosar-uritese vege