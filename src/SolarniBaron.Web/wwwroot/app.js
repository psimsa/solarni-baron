const applicationServerPublicKey =
  'BI-tNOdywyGGMttP_qE-kZbpjqeWz34pj4JWrMcBg99EfisRbH0-w3jSOSU0jh664Zy9gyww_0ayyk3aufpiUnc'

window.blazorPushNotification = {
  requestPermission: async () => {
    return await Notification.requestPermission()
  },
  requestSubscription: async () => {
    if (!'serviceWorker' in navigator) {
      return {
        error: 'Service workers are not supported in this browser.'
      }
    }

    if (!'PushManager' in window) {
      return {
        error: 'PushManager not supported'
      }
    }

    const worker = await navigator.serviceWorker.getRegistration()
    const existingSubscription = await worker.pushManager.getSubscription()
    if (existingSubscription) {
      return existingSubscription
    }
    const newSubscription = await subscribe(worker)
    if (newSubscription) {
      return {
        url: newSubscription.endpoint,
        p256dh: arrayBufferToBase64(newSubscription.getKey('p256dh')),
        auth: arrayBufferToBase64(newSubscription.getKey('auth'))
      }
    }
  },
}

async function subscribe (worker) {
  try {
    return await worker.pushManager.subscribe({
      userVisibleOnly: true,
      applicationServerKey: applicationServerPublicKey
    })
  } catch (error) {
    if (error.name === 'NotAllowedError') {
      return null
    }
    throw error
  }
}

function arrayBufferToBase64 (buffer) {
  // https://stackoverflow.com/a/9458996
  var binary = ''
  var bytes = new Uint8Array(buffer)
  var len = bytes.byteLength
  for (var i = 0; i < len; i++) {
    binary += String.fromCharCode(bytes[i])
  }
  return window.btoa(binary)
}

window.updateAvailable = new Promise((resolve, reject) => {
  navigator.serviceWorker.getRegistration().then(registration => {
    registration.onupdatefound = () => {
      const installingServiceWorker = registration.installing
      installingServiceWorker.onstatechange = () => {
        if (installingServiceWorker.state === 'installed') {
          resolve(!!navigator.serviceWorker.controller)
        }
      }
    }
  })
})

window.registerForUpdateAvailableNotification = (caller, methodName) => {
  window.updateAvailable.then(isUpdateAvailable => {
    if (isUpdateAvailable) {
      caller.invokeMethodAsync(methodName).then()
    }
  })
}
