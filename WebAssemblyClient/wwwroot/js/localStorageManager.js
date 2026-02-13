window.shouldUpdateView = (userId, listingId) => {
    const viewedItems = JSON.parse(localStorage.getItem('listing_views')) || [];

    const userId_listingId = `${userId}_${listingId}`;
    
    if (!viewedItems.includes(userId_listingId)) {
        viewedItems.push(userId_listingId);
        localStorage.setItem('listing_views', JSON.stringify(viewedItems));
        return true;
    }
    return false;
}

window.getViewedListings = () => {
    const data = localStorage.getItem('listing_views');
    return data ? JSON.parse(data) : [];
};