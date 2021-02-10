import React from 'react';
import { BrowserRouter, NavLink, Route, Switch } from "react-router-dom";
import styles from './App.module.css';
import { SearchPage } from "./SearchPage/SearchPage";
import { QueryDetailsPage } from "./QueriesPage/containers/QueryDetailsPage"
import { QueryCreatePage } from "./QueriesPage/containers/QueryCreatePage"

import "@blueprintjs/core/lib/css/blueprint.css";
import "@blueprintjs/datetime/lib/css/blueprint-datetime.css";
import { QueriesPage } from './QueriesPage/QueriesPage';

const linkProps: Partial<React.ComponentProps<typeof NavLink>> = {
    className: styles.navlink,
    activeClassName: styles.navlink_active,
};

const App: React.FC = () => {
    return (
        <BrowserRouter basename={process.env.PUBLIC_URL}>
            <div className={styles.app}>
                <div className={styles.app_header}>
                    <div className={styles.app_title}>Logs Viewer1</div>
                    <NavLink {...linkProps} to={"/"} exact={true}>Logs</NavLink>
                    <NavLink {...linkProps} to={"/queries"}>Queries</NavLink>
                </div>
                <div className={styles.app_content}>
                    <Switch>
                        <Route
                            exact={true}
                            path={"/queries"}
                            render={(props) =>
                                <QueriesPage {...props} />
                            }
                        />
                        <Route
                            exact={true}
                            path={"/queries/:id(\\d+)"}
                            render={(props) =>
                                <QueryDetailsPage {...props} />
                            }
                        />
                        <Route
                            exact={true}
                            path={"/queries/create"}
                            render={(props) =>
                                <QueryCreatePage {...props} />
                            }
                        />
                        <Route path={"/"}>
                            <SearchPage />
                        </Route>
                    </Switch>
                </div>
            </div>
        </BrowserRouter>
    );
}

export default App;
