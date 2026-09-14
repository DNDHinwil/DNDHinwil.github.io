## Codespirals.Blazor.WebsiteTemplate

This is a template from which one can easily build a C# / Blazor / WASM website.

### Why this stack?

- Thanks to being WASM based, this can be hosted for entirely free on GitHub Pages (possibly other free hosts, but I haven't tested those).
- Blazor and Blazor Components are a great framework that is criminally underutilized. 
	- Blazor is **mostly** just enhanced HTML + CSS, which are core web technologies everyone should have enough of a grasp on to use.
	- When properly understood, every Blazor component is basically a self contained, reusable micro-app. It is insanely powerful.
- This setup with compiled WASM as the main frontend language requires *writing* no-to-very-little JavaScript - most code can be written in C#, though there are rare exceptions you might run into.
	- JavaScript does need to be enabled. WASM isn't fully mature enough yet to entirely replace it. Yet.
- This template setup with the attached GitHub workflow allows for automatic building and re-deploying to GitHub Pages on push to your release branch.
- C# is beautiful and easy to write <3

### The downsides

- It's basically a Client-Side only setup. It's of course possible to expand this, but you'll have to set up your own backend and database if you need that. That's not covered in this template.
- C# is not the most *beginner friendly* language. I genuinely love writing code in it and do think it *flows* very well, but there's a learning curve, I admit.
- It's on GitHub. Yeah I mean, come on... I use it because I can host stuff for free, but I'm not gonna bat for Microsoft.

## Instructions

### Grab the code

Go to a repository hosting this code like [GitHub](https://github.com/Codespirals/Codespirals.Blazor.WebsiteTemplate).

Clone the code (on GitHub that's the "<> Code" button).

Open the code in your prefered IDE.

### Rename the project

Since you probably don't want to build "Codespirals.Blazor.WebsiteTemplate", make sure you properly rename the project to whatever you like.

This process is a bit more involved than just renaming the project file - make sure you also remember to rename *namespace* mentions in other .cs files

<aside style="border:1px solid grey;padding: 0.25rem;margin: 0.5rem 0;">
★ A namespace in C# is a very simple way of grouping code. It's defined at the top of the file. Often it follows the folder structure, but it doesn't have to.
Specifiying a namespace allows us to split up code and then call it where we need it by using <code>[Namespace].[myClassOrFunction]</code>
</aside>

Also don't forget the `manifest.webmanifest file`, as that has the names in it too.

In a Blazor project, .razor files like pages by default use the namespace of the file structure they're in, so for example your "Home" page has the default

	namespace [YourProjectName].Pages

if you're ok with that default, then the razor pages do not need any renaming. If not, you can simply write your desired namespace at the top of the .razor file if you wish to use a specific one.

### Rename the TODOs you find

I marked all places where you need to replace soemthing with your own Names / Icons with the common marker "TODO" - if you Ctrl+F for it you will find *most* of them.

### The TODOs you didn't find

The TODO elements you probably won't find are in the GitHub Workflow that comes with this template, as this file is outside the solution folder.

Open your file expolorer, find the root folder of this project and enter .github/workflows/deploy-gh-pages.yml. 

There you should replace the value of the "Project_Location" variable with the relative path to your newly renamed .csproj file.

### CI pipeline

<aside style="border:1px solid grey;padding: 0.25rem;margin: 0.5rem 0;">
★ CI (Continous Integration) is the process of automatically updating and releasing the latest designated build of your software. While you probably don't want to release your work in progress (as things might just not be running in your local code), 
a good CI pipeline lets you easily work on new stuff while making releases as easy as possible. Many developers handle this via Git and Git branches. A branch is designated as the "release" branch, and an update to that branch triggers a refresh of the system.
</aside>

While still in the workflow file, you should designate one of the branches of your project as the "release" branch. This can either simply be the "main" branch, or you can make a new one and name it "release".

Be aware however, that from now on whenever you push to, or pull into this branch, GitHub will rebuild and rerelease your website, so you should probably do your active work on anther branch (or several).

The actual build creates its own branch simply called "gh-pages" - this branch does never need to be touched directly in any way. It simply exists for the workflow to push onto.

However in your repository's settings under "Pages", you need to select this "gh-pages" branch and its "/(root)" folder as the "Deploy from a branch" source of your website.

## Your own domain name

If you have a custom domain you want your site pointed to (instead of just "[my-repository].github.io"), first connect your custom domain on GitHub Pages (they have their own guides on how to do that), then input that domain name in the empty `CNAME` file in the `wwwroot` folder.