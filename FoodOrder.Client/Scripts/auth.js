async function login() {
    var email = document.getElementById('typeEmail').value;
    var password = document.getElementById('typePassword').value;
    if (isEmpty(email) || isEmpty(password)) {
        alert('Email és jelszó megadása kötelező!');
    } else if (!validateEmail(email)) {
        alert('Hibás email formátum!');
    } else {
        var data = {
            email: email,
            password: password
        };
        await postData("auth/login", data, false)
            .then(async (data) => {
                if (await data.token) {
                    localStorage.setItem("data", JSON.stringify(data));
                    window.location.href = "index.html";
                } else {
                    alert(await data.Message);
                }
            });
    }
}

function isEmpty(str) {
    return (!str || 0 === str.length);
}

function validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
}

