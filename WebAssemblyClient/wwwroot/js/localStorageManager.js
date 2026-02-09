window.shouldUpdateView = (id) => {
    const viewedItems = JSON.parse(localStorage.getItem('listing_views')) || [];

    if (!viewedItems.includes(id)) {
        viewedItems.push(id);
        localStorage.setItem('listing_views', JSON.stringify(viewedItems));
        return true;
    }
    return false;
}