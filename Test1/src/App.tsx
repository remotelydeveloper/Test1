import 'es6-shim';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faUserEdit,
  faUserTimes,
  faSignOutAlt,
  faUserPlus,
} from '@fortawesome/free-solid-svg-icons';
library.add(faUserEdit, faUserTimes, faSignOutAlt, faUserPlus);

import {
  BrowserRouter as Router,
  Route,
  Link,
  Switch,
  Redirect,
} from 'react-router-dom';
import { Routes, Roles, StateContext } from './shared';
import { Navbar, Fallback, Forbidden } from '@servicestack/react';

import { Home } from './components/Home';
import { SignIn } from './components/SignIn';
import { signout } from './shared';

export const App: React.FC<any> = () => {
  const { state, dispatch } = React.useContext(StateContext);

  const renderHome = () => <Home name='React' />;

  const requiresAuth = (Component: any, path?: string) => {
    if (!state.userSession) {
      return () => (
        <Redirect
          to={{ pathname: Routes.SignIn, search: '?redirect=' + path }}
        />
      );
    }
    return () => <Component />;
  };
  const requiresRole = (
    role: string,
    Component: React.FC<any>,
    path?: string
  ) => {
    if (!state.userSession) {
      return () => (
        <Redirect
          to={{ pathname: Routes.SignIn, search: '?redirect=' + path }}
        />
      );
    }
    if (!state.userSession.roles || state.userSession.roles.indexOf(role) < 0) {
      return () => <Forbidden path={path} role={role} />;
    }
    return () => <Component />;
  };

  const handleSignOut = async () => await signout(dispatch);

  return (
    <Router>
      <div>
        <nav className='navbar navbar-expand-lg navbar-dark'>
          <div className='container'>
            <Link to='/' className='navbar-brand'>
              <i className='svg-logo svg-lg mr-1' />
              <span className='align-middle'>Test1</span>
            </Link>
            {state.userSession && (
              <div>
                <Link to='/' onClick={handleSignOut} className='navbar-brand'>
                  <FontAwesomeIcon icon='sign-out-alt' />
                  <span className='sign-out-link'>Sign out</span>
                </Link>
              </div>
            )}
          </div>
        </nav>

        <div id='content' className='container mt-4'>
          <Switch>
            <Route
              exact
              path={Routes.Home}
              render={renderHome}
              render={requiresAuth(Home, Routes.Home)}
              activeClassName='active'
            />
            <Route
              path={Routes.SignIn}
              component={SignIn}
              activeClassName='active'
            />
            <Route path={Routes.Forbidden} component={Forbidden} />
            <Route component={Fallback} />
          </Switch>
        </div>
      </div>
    </Router>
  );
};
