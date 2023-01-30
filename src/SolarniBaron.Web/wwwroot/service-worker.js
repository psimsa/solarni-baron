// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', () => {});


self.addEventListener('push',
  function(event) {
    console.log('[Service Worker] Push Received.');
    console.log(`[Service Worker] Push had this data: "${event.data.text()}"`);

    const title = 'Push Codelab';
    const options = {
      body: 'Yay it works.',
      icon: 'images/icon.png',
      badge: 'images/badge.png'
    };

    event.waitUntil(self.registration.showNotification(title, options));
  });

function openPushNotification(event) {
  console.log('Notification click Received.', event.notification.data)
  event.notification.close()
  // do something
}

self.addEventListener('notificationclick', openPushNotification)