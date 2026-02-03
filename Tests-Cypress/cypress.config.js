const { defineConfig } = require("cypress");
const fs = require('fs');
const path = require('path');

function loadLocalEnv() {
    const envPath = path.resolve(
        __dirname,
        'cypress/fixtures/local.env.json'
    );

    if (fs.existsSync(envPath)) {
        const localEnv = JSON.parse(fs.readFileSync(envPath, 'utf8'));
        return localEnv;
    }

    return {};
}

module.exports = 
   defineConfig({
        e2e: {
           baseUrl: process.env.CYPRESS_baseUrl || "http://localhost:8700",
           supportFile: "cypress/support/commands.js",
           viewportWidth: 1280,
           viewportHeight: 800,
           video: false,
           defaultCommandTimeout: 10000,
           setupNodeEvents(on, config) {
               //  Solo carga el JSON local si NO viene desde la integracion con GitHub
               if (!process.env.CYPRESS_USER_NAME) {
                   config.env = {
                       ...config.env,
                       ...loadLocalEnv()
                   };
               }
                return config;
           }
    } });
