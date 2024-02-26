isGet = true;
async function loadJsonData() {
  const jsonContainer = document.getElementById("jsonContainer");
  const jsonDataElement = document.getElementById("jsonData");
  if (isGet) {
    try {
      // getData metódus hívása
      const data = await getData("test");
      console.log(data);
      // JSON adat megjelenítése a HTML-ben
      jsonDataElement.textContent = JSON.stringify(data, null, 2);

      // Collapse megjelenítése
      new bootstrap.Collapse(jsonContainer, { toggle: true });
    } catch (error) {
      alert("Hiba történt az adatok lekérése közben:", error);
    }
    isGet = false;
  } else {
    new bootstrap.Collapse(jsonContainer, { toggle: true });
    isGet = true;
  }
}
