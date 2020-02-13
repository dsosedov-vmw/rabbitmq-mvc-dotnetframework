pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/framework/sdk'
            label 'windows'
        }
    }
    stages {
        stage('Build') {
            steps {
                bat 'nuget restore src'
                bat 'msbuild src'
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
            }
        }
        stage('Publish') {
            steps {
                bat 'msbuild src /p:PublishProfile=FolderProfile /p:DeployOnBuild=true'
            }
        }
        stage('Deploy') {
            steps {
                pushToCloudFoundry(
                  target: 'api.run.pcfone.io',
                  organization: 'dsosedov-pivot',
                  cloudSpace: 'production',
                  credentialsId: 'cf_creds'
                 )
            }
        }
    }
}
