var installer = function () {
    let installPrompt = null;
    let installed: boolean = false;

    window.addEventListener("beforeinstallprompt", (event) => {
        event.preventDefault();
        installPrompt = event;
    });

    window.addEventListener("appinstalled", () => {
        installed = true;
    });
    return {
        installPrompt: async () => {
            if (null == installPrompt) {
                console.log("installPrompt is null");
            } else {
                const result = await installPrompt.prompt();
                console.log(`User response to the install prompt: ${result}`);
            }
        },
        installedOrUnsupported: (): boolean => {
            if (null == installPrompt) {
                console.log("installPrompt is null, maybe unsupported");
                return false;
            }
            if (installed) {
                console.log("installed");
                return false;
            }
            return true;
        }
    }
}();
window.installer = installer;