using APBD_TASK2.App;

// Program.cs is intentionally a thin entry point. All wiring lives in the
// Bootstrapper (composition root) and all behaviour lives in the services and the
// DemoScenario, so the console layer carries no business logic.
Bootstrapper.BuildDemo().Run();
