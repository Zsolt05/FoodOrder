let id = 0;
document.addEventListener('DOMContentLoaded', async function () {
    // Populate categories select
    await fetchCategories();

    // Check if URL has an ID parameter
    const urlParams = new URLSearchParams(window.location.search);
    id = urlParams.get('id');
    if (id) {
        document.title = "Étel módosítása- FoodOrder";
    }

    // Form submit event listener
    document.getElementById('foodForm').addEventListener('submit', function (event) {
        event.preventDefault();
        const formData = new FormData(this);
        createFood(formData);
    });
});

// Function to fetch food categories
async function fetchCategories() {
    let categories = await getData('food/category?pageNumber=1&pageSize=10');
    const categorySelect = document.getElementById('category');
    categories.items.forEach(category => {
        const option = document.createElement('option');
        option.value = category.id;
        option.textContent = category.name;
        categorySelect.appendChild(option);
    });
}

// Function to create food
async function createFood(formData) {
    if (id !== 0)
    {
        await putData('food', formData);
    }
    fetch('/api/food/create', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (response.ok) {
                alert('Food created successfully!');
                window.location.href = '/'; // Redirect to homepage after successful creation
            } else {
                throw new Error('Failed to create food');
            }
        })
        .catch(error => console.error('Error creating food:', error));
}