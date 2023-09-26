import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { Provider } from 'react-redux';
import store from './app/state/store';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Provider store={store}>
        <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Routes>
        </Layout>
      </Provider>
    );
  }
}
