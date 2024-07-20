const constructListComponent = async (root) => {
    let data;

    const resp = await fetch("./items");
    data = await resp.json();

    const renderItem = (item) => `
        <li>
            <input type="checkbox" name="items" id="${item.id}" value="${item.id}" />
            <label for="${item.id}">
                <div>${item.name}</div>
                <div>${item.description}</div>
            </label>
        </li>`;

    const render = () => {
        let html = data.map(d => renderItem(d)).join("");

        root.innerHTML = html;
    };

    render();
}

const handleFormSubmit = async (event) => {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const selectedItems = formData.getAll('items');

    const resp = await fetch("./items", {
        method: 'POST',
        body: new URLSearchParams(selectedItems.map(id => ['items', id]))
    });

    const returnedItems = await resp.json();

    const renderReturnedItems = (items) => `
        <div class="returned-items">
            <h2>Returned Items</h2>
            <ul>
                ${items.map(item => `
                    <li>
                        <div>${item.name}</div>
                        <div>${item.description}</div>
                    </li>`).join('')}
            </ul>
        </div>`;

    const returnedItemsContainer = document.querySelector('.returned-items-container');
    returnedItemsContainer.innerHTML = renderReturnedItems(returnedItems);
}

constructListComponent(document.querySelector("ul"));
document.querySelector("form").addEventListener("submit", handleFormSubmit);
