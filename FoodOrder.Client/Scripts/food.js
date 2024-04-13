let id = 0;
document.addEventListener('DOMContentLoaded', async function () {
    // Fetch food categories
    await fetchCategories();
    // Check if URL has an ID parameter
    const urlParams = new URLSearchParams(window.location.search);
    id = urlParams.get('id');
    if (id) {
        document.title = "Étel módosítása- FoodOrder";
        const food = await getData(`food/${id}`);
        document.getElementById('name').value = food.name;
        document.getElementById('price').value = food.price;
        document.getElementById('categoryId').value = food.category.id;
    }
    // Form submit event listener
    document.getElementById('foodForm').addEventListener('submit', function (event) {
        event.preventDefault();
        const formData = new FormData(this);
        const createFoodDto = {};
        formData.forEach((value, key) => {
            createFoodDto[key] = value;
        });
        if (createFoodDto.categoryId === "") {
            alert("Kérlek válassz kategóriát!");
        }
        createFood(createFoodDto);
    });
});

// Function to fetch food categories
async function fetchCategories() {
    let categories = await getData('food/category?pageNumber=1&pageSize=10');
    const categorySelect = document.getElementById('categoryId');
    categories.items.forEach(category => {
        const option = document.createElement('option');
        option.value = category.id;
        option.textContent = category.name;
        categorySelect.appendChild(option);
    });
}

// Function to create/update food
async function createFood(formData) {
    console.log(formData);
    if (id !== 0) {
        await putData(`food/${id}`, formData);
        alert("Sikeres módosítás!");
        window.location.href = 'index.html';
    } else {
        await postData('food', formData);
        alert("Sikeres hozzáadás!");
        window.location.href = 'index.html';
    }
}