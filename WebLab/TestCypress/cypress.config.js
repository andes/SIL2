const { defineConfig } = require("cypress");

module.exports = 
   defineConfig({
        e2e: {
           baseUrl: "http://localhost:8700",
            
            supportFile: "cypress/support/commands.js",
            viewportWidth: 1280,
            viewportHeight: 800,
            video: false,
            defaultCommandTimeout: 10000
    } });
