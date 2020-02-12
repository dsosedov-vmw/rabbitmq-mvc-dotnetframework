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
                bar 'msbuild src'
            }
        }
        stage('Test') {
            steps {
                bat 'Testing...'
            }
        }
        stage('Publish') {
            steps {
                bat 'Publishing...'
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
