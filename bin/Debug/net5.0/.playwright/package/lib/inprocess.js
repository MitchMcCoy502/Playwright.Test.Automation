"use strict";

var _dispatcher = require("./dispatchers/dispatcher");

var _playwright = require("./server/playwright");

var _playwrightDispatcher = require("./dispatchers/playwrightDispatcher");

var _connection = require("./client/connection");

var _browserServerImpl = require("./browserServerImpl");

/**
 * Copyright (c) Microsoft Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the 'License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
function setupInProcess() {
  const playwright = (0, _playwright.createPlaywright)();
  const clientConnection = new _connection.Connection();
  const dispatcherConnection = new _dispatcher.DispatcherConnection(); // Dispatch synchronously at first.

  dispatcherConnection.onmessage = message => clientConnection.dispatch(message);

  clientConnection.onmessage = message => dispatcherConnection.dispatch(message); // Initialize Playwright channel.


  new _playwrightDispatcher.PlaywrightDispatcher(dispatcherConnection.rootDispatcher(), playwright);
  const playwrightAPI = clientConnection.getObjectWithKnownName('Playwright');
  playwrightAPI.chromium._serverLauncher = new _browserServerImpl.BrowserServerLauncherImpl('chromium');
  playwrightAPI.firefox._serverLauncher = new _browserServerImpl.BrowserServerLauncherImpl('firefox');
  playwrightAPI.webkit._serverLauncher = new _browserServerImpl.BrowserServerLauncherImpl('webkit'); // Switch to async dispatch after we got Playwright object.

  dispatcherConnection.onmessage = message => setImmediate(() => clientConnection.dispatch(message));

  clientConnection.onmessage = message => setImmediate(() => dispatcherConnection.dispatch(message));

  playwrightAPI._toImpl = x => dispatcherConnection._dispatchers.get(x._guid)._object;

  return playwrightAPI;
}

module.exports = setupInProcess();
//# sourceMappingURL=inprocess.js.map