import React from "react";

const HomePage = () => {
  return (
    <div className="jumbotron">
      <h2 className="text-center">el Restaurante</h2>
      <img src="https://banner2.kisspng.com/20180407/pje/kisspng-cafeteria-restaurant-clip-art-construction-site-5ac84c25707286.3235905415230761334606.jpg"
           className="img-fluid" alt="restaurant"/>
      <p className="text-center">
        A sample application built using Domain-Driven Design and .NET Core.
      </p>
      <div class="col text-center">
      <a
        href="https://github.com/profjordanov/restaurant"
        className="btn btn-primary btn-md justify-content-center">
        Check out the source code
      </a>
    </div>
    </div>
  );
};

export default HomePage;
