import React from 'react';
import { BrowserRouter, NavLink, Route, Switch } from "react-router-dom";
import styles from './App.module.css';
import { QueriesPage } from "./QueriesPage/QueriesPage";
import { SearchPage } from "./SearchPage/SearchPage";

import "@blueprintjs/core/lib/css/blueprint.css";
import "@blueprintjs/datetime/lib/css/blueprint-datetime.css";

const linkProps: Partial<React.ComponentProps<typeof NavLink>> = {
  className: styles.navlink,
  activeClassName: styles.navlink_active,
};

const App: React.FC = () => {
    return (
        <BrowserRouter>
            <div className={styles.app}>
                <div className={styles.app_header}>
                  <div className={styles.app_title}>Logs Viewer1</div>
                  <NavLink {...linkProps} to={"/"} exact={true}>Logs</NavLink>
                  <NavLink {...linkProps} to={"/queries"}>Queries</NavLink>
                </div>
                <div className={styles.app_content}>
                    <Switch>
                        <Route path={"/queries"}>
                            <QueriesPage/>
                        </Route>
                        <Route path={"/"}>
                            <SearchPage/>
                        </Route>
                    </Switch>
                </div>
            </div>
        </BrowserRouter>
    );
}

export default App;
