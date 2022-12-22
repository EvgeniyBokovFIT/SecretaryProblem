using PrincessHttpClient;

PrincessClient princessClient = new PrincessClient(new StrategyClient());
princessClient.DoSeveralTries();
