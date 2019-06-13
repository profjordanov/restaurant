import React from "react";
import Header from "./common/Header";
import PageNotFound from "./PageNotFound";
import HomePage from "./home/HomePage";
import LoginPage from "./auth/LoginPage";
import Logout from "./auth/Logout";
import { Route, Switch } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import UserProvider from "./UserProvider";
import "./font-awesome.css";

const App = () => {
  return (
    <div className="container-fluid">
      <Header />
      <Switch>
        <Route exact path="/" component={HomePage} />
        <Route path="/login" component={LoginPage} />
        <Route path="/logout" component={Logout} />     
        <Route component={PageNotFound} />
      </Switch>
      <UserProvider />
      <ToastContainer autoClose={3000} hideProgressBar={true} />
    </div>
  );
};

export default App;
